// *****************************************************************************************************************
// Project          : Navyblue
// File             : BaseApiController.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:54
// *****************************************************************************************************************
// <copyright file="BaseApiController.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Navyblue.Authorization.Extensions;
using Navyblue.BaseLibrary;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Navyblue.Authorization.Authorizations;

namespace Navyblue.Authorization;

/// <summary>
/// BaseApiController.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
[ApiController]
public abstract class AuthApiController : ControllerBase
{
    /// <summary>
    /// The access token protector
    /// </summary>
    private static readonly Lazy<AccessTokenProtector> accessTokenProtector = new(() => InitAccessTokenProtector());
    /// <summary>
    /// The bearer authentication keys
    /// </summary>
    private static string _bearerAuthKeys;
    /// <summary>
    /// The authentication configuration
    /// </summary>
    private readonly AuthorizationConfig _authConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthApiController" /> class.
    /// </summary>
    /// <param name="authConfigOptions">The authentication configuration options.</param>
    protected AuthApiController(IOptions<AuthorizationConfig> authConfigOptions)
    {
        this._authConfig = authConfigOptions.Value;
        _bearerAuthKeys = this._authConfig.PrivateKey;
    }

    /// <summary>
    /// Gets the access token protector.
    /// </summary>
    /// <value>
    /// The access token protector.
    /// </value>
    private AccessTokenProtector AccessTokenProtector => accessTokenProtector.Value;

    /// <summary>
    /// Builds the authentication token.
    /// </summary>
    /// <param name="claims">The claims.</param>
    /// <param name="schemeName">Name of the scheme.</param>
    /// <param name="expirationSeconds">The expiration seconds.</param>
    /// <returns>
    /// System.String.
    /// </returns>
    protected string BuildAuthToken(List<Claim> claims, string schemeName, long expirationSeconds = 0)
    {
        claims.Add(new Claim(ClaimTypes.Expiration, expirationSeconds == 0 ? this.GetExpiryTimestamp().ToString() : expirationSeconds.ToString()));
        ClaimsIdentity identity = new(claims, schemeName);

        return this.AccessTokenProtector.Protect(identity);
    }

    /// <summary>
    /// Gets the expiry timestamp. 901 is Android， 902 is IOS
    /// How long the token will be effective
    /// </summary>
    /// <returns>
    /// System.Int64.
    /// </returns>
    protected long GetExpiryTimestamp()
    {
        int duration = this._authConfig.PcSignInExpirationSeconds;
        TraceEntry traceEntry = this.Request.GetTraceEntry();
        if (traceEntry.ClientId.Contains(this._authConfig.IosClientId) || traceEntry.ClientId.Contains(this._authConfig.AndroidClientId))
        {
            duration = this._authConfig.AppSignInExpirationSeconds;
        }
        else if (HttpRequestExtensions.IsFromMobileDevice(this.HttpContext))
        {
            duration = this._authConfig.MobileSignInExpirationSeconds;
        }

        return DateTime.UtcNow.Add(duration.Seconds()).UnixTimestamp();
    }

    /// <summary>
    /// Initializes the jym access token protector.
    /// </summary>
    /// <returns>
    /// AccessTokenProtector.
    /// </returns>
    private static AccessTokenProtector InitAccessTokenProtector()
    {
        return new AccessTokenProtector(_bearerAuthKeys.HtmlDecode());
    }
}