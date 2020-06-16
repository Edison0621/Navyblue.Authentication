// *****************************************************************************************************************
// Project          : Navyblue
// File             : ActionParameterValidateAttribute.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:53
// *****************************************************************************************************************
// <copyright file="ActionParameterValidateAttribute.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Navyblue.BaseLibrary;

namespace Navyblue.Authorization.Filters
{
    /// <summary>
    ///     An action filter for validating action parameter, if validate failed, create a 400 response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ActionParameterValidateAttribute : OrderedActionFilterAttribute
    {
        /// <summary>
        ///     Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                StringBuilder errors = new StringBuilder();

                foreach (KeyValuePair<string, ModelStateEntry> keyValuePair in actionContext.ModelState)
                {
                    foreach (ModelError modelError in keyValuePair.Value.Errors)
                    {
                        errors.Append(modelError.ErrorMessage);
                    }
                }

                actionContext.HttpContext.Response.WriteAsync(new { Message = errors.ToString() }.ToJson(), Encoding.UTF8);

                actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}