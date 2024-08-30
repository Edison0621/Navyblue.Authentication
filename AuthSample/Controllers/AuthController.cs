using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Navyblue.Authorization;
using Navyblue.Authorization.Authorizations;
using Navyblue.Authorization.Authorizations.Applications;

namespace AuthSample.Controllers;

/// <summary>
/// Class AuthController.
/// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller" />
/// </summary>
/// <seealso cref="Navyblue.Authorization.AuthApiController" />
/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
[Route("[controller]")]
public class AuthController : AuthApiController
{
    /// <summary>
    /// The RSA
    /// </summary>
    private readonly RSA _rsa;

    /// <summary>
    /// The HTTP client factory
    /// </summary>
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController" /> class.
    /// </summary>
    /// <param name="authConfigOptions">The authentication configuration options.</param>
    /// <param name="rsa">The RSA.</param>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    /// <param name="serviceProvider"></param>
    public AuthController(IOptions<AuthorizationConfig> authConfigOptions, RSA rsa, IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider) : base(authConfigOptions)
    {
        this._rsa = rsa;
        this._httpClientFactory = httpClientFactory;
        this._serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Gets the authentication token.
    /// </summary>
    /// <returns>
    /// IActionResult.
    /// </returns>
    [HttpGet]
    [AllowAnonymous]
    [Route("GetAuthToken")]
    public IActionResult GetAuthToken()
    {
        string userIdentify = "111111111111111111111111111111111";
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, userIdentify)
        ];

        string authToken = this.BuildAuthToken(claims, AuthorizationScheme.BEARER);
        return this.Ok(authToken);
    }

    /// <summary>
    /// Gets the authentication token.
    /// </summary>
    /// <returns>
    /// IActionResult.
    /// </returns>
    [HttpGet]
    [UserAuthorize]
    [Route("TestAuthToken")]
    public IActionResult TestAuthToken()
    {
        return this.Ok(this.User.Identity?.Name);
    }
    /// <summary>
    /// Gets the authentication token.
    /// </summary>
    /// <returns>
    /// IActionResult.
    /// </returns>
    [HttpGet]
    [AllowAnonymous]
    [Route("GetAppAuthToken")]
    public IActionResult GetAppAuthToken()
    {
        string token = TokenGenerator.GenerateInternalToken("AuthSample", "Application", "otherInfo", 86400, this._rsa);
        return this.Ok(token);
    }

    /// <summary>
    /// Gets the authentication token.
    /// </summary>
    /// <returns>
    /// IActionResult.
    /// </returns>
    [HttpGet]
    [ApplicationAuthorize]
    [Route("TestAuthToken2")]
    public IActionResult TestAuthToken2()
    {
        return this.Ok(this.User.Identity?.Name);
    }

    /// <summary>
    /// Gets the authentication token.
    /// </summary>
    /// <returns>
    /// IActionResult.
    /// </returns>
    [HttpGet]
    [Route("NonAuthTest")]
    public IActionResult NonAuthTest()
    {
        return this.Ok(this.User.Identity?.Name);
    }

    /// <summary>
    /// Fetches the internal data.
    /// </summary>
    /// <returns></returns>
    [HttpGet("fetch-internal-data")]
    public async Task<IActionResult> FetchInternalData()
    {
        HttpClient client = this._httpClientFactory.CreateInternalClient(this._serviceProvider, "ApplicationName");
        HttpResponseMessage response = await client.GetAsync("api/yourcontroller/internal");
        string result = await response.Content.ReadAsStringAsync();

        return this.Ok(result);
    }
}