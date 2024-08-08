using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
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
    /// Initializes a new instance of the <see cref="AuthController" /> class.
    /// </summary>
    /// <param name="authConfigOptions">The authentication configuration options.</param>
    /// <param name="rsa">The RSA.</param>
    public AuthController(IOptions<AuthorizationConfig> authConfigOptions, RSA rsa):base(authConfigOptions)
    {
        this._rsa = rsa;
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
}