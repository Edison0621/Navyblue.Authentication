using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Navyblue.Authentication;
using Navyblue.Authentication.Extensions;
using Navyblue.Authentication.Filters;
using Navyblue.BaseLibrary;

namespace AuthTest.Controllers
{
    /// <summary>
    /// Class AuthController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class AuthController : BaseApiController
    {
        /// <summary>
        /// The jym access token protector
        /// </summary>
        private static readonly Lazy<AccessTokenProtector> accessTokenProtector = new Lazy<AccessTokenProtector>(() => InitJYMAccessTokenProtector());

        /// <summary>
        /// The authentication configuration
        /// </summary>
        private readonly AuthorizationConfig _authConfig;

        /// <summary>
        /// The bearer authentication keys
        /// </summary>
        private static string bearerAuthKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authConfigOptions">The authentication configuration options.</param>
        public AuthController(IOptions<AuthorizationConfig> authConfigOptions)
        {
            this._authConfig = authConfigOptions.Value;
            bearerAuthKeys = this._authConfig.BearerAuthKeys;
        }

        /// <summary>
        /// Gets the access token protector.
        /// </summary>
        /// <value>The access token protector.</value>
        private AccessTokenProtector AccessTokenProtector => accessTokenProtector.Value;

        /// <summary>
        /// Gets the authentication token.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAuthToken")]
        public IActionResult GetAuthToken()
        {
            string authToken = this.BuildAuthToken(Guid.NewGuid().ToGuidString(), AuthorizationScheme.Bearer);
            return this.Ok(authToken);
        }

        /// <summary>
        /// Gets the authentication token.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        [UserAuthorize]
        [Route("TestAuthToken")]
        public IActionResult TestAuthToken()
        {
            return this.Ok("Tested");
        }

        /// <summary>
        /// Builds the authentication token.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <returns>System.String.</returns>
        protected string BuildAuthToken(string userIdentifier, string schemeName)
        {
            ClaimsIdentity identity = this.BuildPrincipal(userIdentifier, schemeName);
            return this.AccessTokenProtector.Protect(identity);
        }

        /// <summary>
        /// Initializes the jym access token protector.
        /// </summary>
        /// <returns>AccessTokenProtector.</returns>
        private static AccessTokenProtector InitJYMAccessTokenProtector()
        {
            return new AccessTokenProtector(bearerAuthKeys.HtmlDecode());
        }

        /// <summary>
        /// Builds the principal.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <param name="expirationSeconds">The expiration seconds.</param>
        /// <returns>ClaimsIdentity.</returns>
        protected ClaimsIdentity BuildPrincipal(string userIdentifier, string schemeName, int expirationSeconds = 0)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userIdentifier),
                new Claim(ClaimTypes.Expiration, expirationSeconds==0? this.GetExpiryTimestamp().ToString():expirationSeconds.ToString())
            };

            //ClaimsIdentity identity = new ClaimsIdentity(claims, schemeName);
            //this.User = new ClaimsPrincipal(identity);

            return new ClaimsIdentity(claims, schemeName);
        }

        /// <summary>
        /// Gets the expiry timestamp.
        /// </summary>
        /// <returns>System.Int64.</returns>
        protected long GetExpiryTimestamp()
        {
            int duration = this._authConfig.PCSignInExpirationSeconds;
            TraceEntry traceEntry = this.Request.GetTraceEntry();
            if (traceEntry.ClientId.Contains("901") || traceEntry.ClientId.Contains("902"))
            {
                duration = this._authConfig.AppSignInExpirationSeconds;
            }
            else if (HttpRequestExtensions.IsFromMobileDevice(this.HttpContext))
            {
                duration = this._authConfig.MobileSignInExpirationSeconds;
            }

            return DateTime.UtcNow.Add(duration.Seconds()).UnixTimestamp();
        }
    }
}
