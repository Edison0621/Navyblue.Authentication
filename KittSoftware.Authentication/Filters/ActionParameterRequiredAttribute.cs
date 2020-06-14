// *****************************************************************************************************************
// Project          : Navyblue
// File             : ActionParameterRequiredAttribute.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:53
// *****************************************************************************************************************
// <copyright file="ActionParameterRequiredAttribute.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Navyblue.Authentication.Filters
{
    /// <summary>
    ///     An action filter for checking whether the action parameter is null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ActionParameterRequiredAttribute : OrderedActionFilterAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActionParameterRequiredAttribute" /> class.
        /// </summary>
        /// <param name="actionParameterName">Name of the action parameter.</param>
        /// <exception cref="System.ArgumentNullException">actionParameterName</exception>
        public ActionParameterRequiredAttribute(string actionParameterName = "request")
        {
            this.ActionParameterName = actionParameterName;
        }

        /// <summary>
        ///     Gets the name of the action parameter.
        /// </summary>
        /// <value>The name of the action parameter.</value>
        public string ActionParameterName { get; }

        /// <summary>
        ///     Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ActionArguments.TryGetValue(this.ActionParameterName, out object parameterValue))
            {
                if (parameterValue == null)
                {
                    string errorMessage = this.FormatErrorMessage();
                    actionContext.ModelState.AddModelError(this.ActionParameterName, errorMessage);
                    //actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessage);
                    actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    //TODO actionContext.HttpContext.Response.WriteAsync(errorMessage);
                }
            }
        }

        /// <summary>
        ///     Formats the error message.
        /// </summary>
        /// <returns>System.String.</returns>
        private string FormatErrorMessage()
        {
            return $"The action parameter {this.ActionParameterName} cannot be null.";
        }
    }
}