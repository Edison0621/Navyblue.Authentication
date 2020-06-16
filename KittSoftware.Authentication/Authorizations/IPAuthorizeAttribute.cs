// *****************************************************************************************************************
// Project          : Navyblue
// File             : IPAuthorizeAttribute.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:54
// *****************************************************************************************************************
// <copyright file="IPAuthorizeAttribute.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Text.RegularExpressions;
using Navyblue.Authorization.Filters;
using Navyblue.Authorization.Extensions;

namespace Navyblue.Authentication.Authorizations
{
    /// <summary>
    ///     IPAuthorizeAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IPAuthorizeAttribute : OrderedAuthorizationFilterAttribute
    {
        /// <summary>
        ///     Gets or sets the valid ip regex.
        /// </summary>
        /// <value>The valid ip regex.</value>
        public Regex ValidIPRegex { get; set; }

        /// <summary>
        ///     Calls when a process requests authorization.
        /// </summary>
        /// <param name="actionContext">The action context, which encapsulates information for using <see cref="T:System.Web.Http.Filters.AuthorizationFilterAttribute" />.</param>
        public override void OnAuthorization(AuthorizationFilterContext actionContext)
        {
            if (!this.IpIsAuthorized(actionContext))
            {
                this.HandleUnauthorizedRequest(actionContext);
                return;
            }

            base.OnAuthorization(actionContext);
        }

        /// <summary>
        ///     Processes requests that fail authorization. This default implementation creates a new response with the
        ///     Unauthorized status code. Override this method to provide your own handling for unauthorized requests.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        /// <exception cref="System.ArgumentNullException">@actionContext can not be null</exception>
        private void HandleUnauthorizedRequest(AuthorizationFilterContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext), @"actionContext can not be null");
            }

            //actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "");

            actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }

        /// <summary>
        ///     Ips the is authorized.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool IpIsAuthorized(AuthorizationFilterContext context)
        {
            HttpRequest request = context.HttpContext.Request;
            string ip = HttpRequestExtensions.GetUserHostAddress(request.HttpContext.Request);

            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }

            if (this.ValidIPRegex == null)
            {
                return ip == "::1";
            }

            return this.ValidIPRegex.IsMatch(ip) || ip == "::1";
        }
    }
}