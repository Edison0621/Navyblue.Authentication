using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Navyblue.Authentication.Authorizations;
using Navyblue.Authorization;
using Navyblue.Authorization.Extensions;
using Navyblue.BaseLibrary;

namespace AuthTest.Controllers
{
    /// <summary>
    /// Class AuthController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class AuthController : AuthApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authConfigOptions">The authentication configuration options.</param>
        public AuthController(IOptions<AuthorizationConfig> authConfigOptions):base(authConfigOptions)
        {
        }

        /// <summary>
        /// Gets the authentication token.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAuthToken")]
        public IActionResult GetAuthToken()
        {
            string userIdentify = "111111111111111111111111111111111";
            string authToken = this.BuildAuthToken(userIdentify, AuthorizationScheme.Bearer);
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
            return this.Ok(this.User.Identity.Name);
        }
    }
}
