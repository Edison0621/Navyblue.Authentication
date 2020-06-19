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
using Navyblue.Authentication.Authorizations;
using Navyblue.Authorization.Extensions;
using Navyblue.BaseLibrary;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Navyblue.Authorization
{
    /// <summary>
    ///     BaseApiController.
    /// </summary>
    public abstract class AuthApiController : Controller
    {
        private static readonly Lazy<AccessTokenProtector> accessTokenProtector = new Lazy<AccessTokenProtector>(() => InitAccessTokenProtector());
        private static string bearerAuthKeys;
        private readonly AuthorizationConfig _authConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authConfigOptions">The authentication configuration options.</param>
        public AuthApiController(IOptions<AuthorizationConfig> authConfigOptions)
        {
            this._authConfig = authConfigOptions.Value;
            bearerAuthKeys = this._authConfig.BearerAuthKeys;
        }

        private AccessTokenProtector AccessTokenProtector => accessTokenProtector.Value;

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
                new Claim(ClaimTypes.Name, userIdentifier),//TODO: you can add other property
                new Claim(ClaimTypes.Expiration, expirationSeconds==0? this.GetExpiryTimestamp().ToString():expirationSeconds.ToString())
            };

            return new ClaimsIdentity(claims, schemeName);
        }

        /// <summary>
        /// Gets the expiry timestamp. 901 is Android， 902 is IOS
        /// How long the token will be effective
        /// </summary>
        /// <returns>System.Int64.</returns>
        protected long GetExpiryTimestamp()
        {
            int duration = this._authConfig.PCSignInExpirationSeconds;
            TraceEntry traceEntry = this.Request.GetTraceEntry();
            if (traceEntry.ClientId.Contains(_authConfig.IOSClientId) || traceEntry.ClientId.Contains(_authConfig.AndroidClientId))
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
        /// <returns>AccessTokenProtector.</returns>
        private static AccessTokenProtector InitAccessTokenProtector()
        {
            return new AccessTokenProtector(bearerAuthKeys.HtmlDecode());
        }
    }
}