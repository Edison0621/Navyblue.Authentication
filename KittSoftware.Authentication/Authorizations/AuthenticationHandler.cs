// ******************************************************************************************************
// Project          : AuthTest
// File             : BasicAuthenticationHandler.cs
// Created          : 2020-06-14  22:42
// 
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2020-06-14  22:42
// ******************************************************************************************************
// <copyright file="BasicAuthenticationHandler.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2020 Shanghai Future  Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Navyblue.Authorization.Extensions;
using Navyblue.BaseLibrary;
using Newtonsoft.Json.Linq;

namespace Navyblue.Authorization.Authorizations;

public class AuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
{
    /// <summary>
    ///     Gets a value indicating whether [use swagger as application for dev].
    /// </summary>
    /// <value><c>true</c> if [use swagger as application for dev]; otherwise, <c>false</c>.</value>
    public bool UseSwaggerAsApplicationForDev { get; }

    /// <summary>
    ///     Gets or sets the government server public key.
    /// </summary>
    /// <value>The government server public key.</value>
    public string GovernmentServerPublicKey { get; set; }

    private RSA CryptoServiceProvider
    {
        get
        {
            if (this.GovernmentServerPublicKey.IsNullOrEmpty())
            {
                return null;
            }

            try
            {
                RSA rsa = RSA.Create();
                rsa.FromXmlString(this.GovernmentServerPublicKey);

                return rsa;
            }
            catch (Exception e)
            {
                throw new Exception("Bad format key with {0}".FormatWith(this.GovernmentServerPublicKey), e);
            }
        }
    }

    private ClaimsIdentity Identity
    {
        get => this._httpContext?.User.Identity as ClaimsIdentity;
        set
        {
            if (this._httpContext != null)
            {
                this._httpContext.User = new ClaimsPrincipal(value);
            }
        }
    }

    private List<string> AllowedLists { get; }

    private readonly HttpContext _httpContext;
    private readonly AuthorizationConfig _authConfig;
    private readonly AccessTokenProtector _accessTokenProtector;

    public AuthenticationHandler(
        IOptionsMonitor<BasicAuthenticationOptions> options,
        IHttpContextAccessor httpContextAccessor,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IOptions<AuthorizationConfig> authConfigOptions)
        : base(options, logger, encoder, clock)
    {
        this._httpContext = httpContextAccessor.HttpContext;
        this._authConfig = authConfigOptions.Value;
        this._accessTokenProtector = new AccessTokenProtector(this._authConfig.BearerAuthKeys.HtmlDecode());
        this.GovernmentServerPublicKey = this._authConfig.GovernmentServerPublicKey;
        this.UseSwaggerAsApplicationForDev = this._authConfig.UseSwaggerAsApplicationForDev;
        this.AllowedLists = this._authConfig.AllowedLists;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (this.HasAuthorizationHeader(this._httpContext.Request) && this._httpContext.Request.Headers[this._authConfig.AuthHeaderName] != StringValues.Empty)
        {
            this.AuthorizeUserViaBearerToken(this._httpContext.Request);
        }
        else if (this.GovernmentServerPublicKey != null && this.HasAuthorizationHeader(this._httpContext.Request, AuthorizationScheme.INTERNAL_AUTH))
        {
            this.AuthorizeApplicationViaAuthToken(this._httpContext.Request);
        }
        else if (this.UseSwaggerAsApplicationForDev && this.IsFromSwagger(this._httpContext.Request))
        {
            this.AuthorizeApplicationIfFromSwagger();
        }
        else if (this.IsFromIPAllowedLists(this._httpContext.Request))
        {
            this.AuthorizeApplicationIfFromIPAllowedLists(this._httpContext.Request);
        }
        else if (HttpRequestExtensions.IsFromLocalhost(this._httpContext).HasValue)
        {
            this.AuthorizeApplicationIfFromLocalhost();
        }

        if (this.HasAuthorizationHeader(this._httpContext.Request)
            && this._httpContext.Request.Headers[this._authConfig.AuthHeaderName] == StringValues.Empty
            && this.Identity != null
            && this.Identity.IsAuthenticated
            && this.Identity.AuthenticationType == AuthorizationScheme.BEARER
            && this._httpContext.Response.StatusCode == (int)HttpStatusCode.OK)
        {
            await this.GenerateAndSetAccessToken();
        }

        ClaimsPrincipal principal = new(this.Identity);
        AuthenticationTicket ticket = new(principal, this.Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private bool HasAuthorizationHeader(HttpRequest request, string scheme = "Bearer")
    {
        return request.Headers[this._authConfig.AuthHeaderName] != StringValues.Empty && request.Headers[this._authConfig.AuthHeaderName].ToString().Contains(scheme);
    }

    private void AuthorizeApplicationIfFromLocalhost()
    {
        this.Identity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Name, "Localhost"),
            new(ClaimTypes.Role, "Application")
        }, AuthorizationScheme.INTERNAL_AUTH);
    }

    private void AuthorizeApplicationIfFromSwagger()
    {
        this.Identity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Name, "Swagger"),
            new(ClaimTypes.Role, "Application")
        }, AuthorizationScheme.INTERNAL_AUTH);
    }

    private void AuthorizeApplicationIfFromIPAllowedLists(HttpRequest request)
    {
        string ip = request.Host.Host;
        this.Identity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Name, $"IP: {ip}"),
            new(ClaimTypes.Role, "Application")
        }, AuthorizationScheme.INTERNAL_AUTH);
    }

    private void AuthorizeApplicationViaAuthToken(HttpRequest request)
    {
        string token = request.Headers[this._authConfig.AuthHeaderName].ToString().ToBase64Bytes().ASCII();
        string[] tokenPiece = token?.Split(',');
        if (tokenPiece?.Length == 5)
        {
            string ticket = tokenPiece.Take(4).Join(",");
            string sign = tokenPiece[4];

            if (this.CryptoServiceProvider.VerifyData(ticket.GetBytesOfASCII(), sign.ToBase64Bytes(), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1))
            {
                if (tokenPiece[3].AsLong(0) > DateTime.UtcNow.UnixTimestamp() && tokenPiece[1] == "") //TODO App.Host.Role)
                {
                    this.Identity = new ClaimsIdentity(new List<Claim>
                    {
                        new(ClaimTypes.Name, tokenPiece[0]),
                        new(ClaimTypes.Role, "Application")
                    }, AuthorizationScheme.INTERNAL_AUTH);
                }
            }
        }
    }

    private void AuthorizeUserViaBearerToken(HttpRequest request)
    {
        this.Identity = this._accessTokenProtector.Unprotect(request.Headers[this._authConfig.AuthHeaderName].ToString().Remove(0, this.Scheme.Name.Length + 1));
    }

    /// <summary>
    /// Generates the and set access token.
    /// </summary>
    /// <returns></returns>
    private async Task GenerateAndSetAccessToken()
    {
        Claim claim = this.Identity.FindFirst(ClaimTypes.Expiration);
        long timestamp = claim?.Value.AsLong() ?? DateTime.UtcNow.UnixTimestamp();

        if (this._httpContext.Response.ContentType == "application/json")
        {
            StreamReader sr = new(this._httpContext.Response.Body);
            string content = await sr.ReadToEndAsync();

            JObject jObject = JObject.Parse(content);
            jObject.Remove("access_token");
            jObject.Remove("expiration");
            jObject.Add("access_token", this._accessTokenProtector.Protect(this.Identity));
            jObject.Add("expiration", timestamp);

            this._httpContext.Response.Clear();
            this._httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await this._httpContext.Response.WriteAsync(jObject.ToJson());
        }
    }

    private bool IsFromSwagger(HttpRequest request)
    {
        if (request.Headers[HeaderNames.Referer] != StringValues.Empty)
        {
            return request.Headers[HeaderNames.Referer].ToString().Contains("swagger", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    private bool IsFromIPAllowedLists(HttpRequest request)
    {
        return this.AllowedLists != null && this.AllowedLists.Contains(request.Host.Host);
    }
}

public class BasicAuthenticationOptions : AuthenticationSchemeOptions
{
    public string Realm { get; set; }
}

public class BasicAuthenticationPostConfigureOptions : IPostConfigureOptions<BasicAuthenticationOptions>
{
    public void PostConfigure(string name, BasicAuthenticationOptions options)
    {
        if (string.IsNullOrEmpty(options.Realm))
        {
            throw new InvalidOperationException("Realm must be provided in options");
        }
    }
}