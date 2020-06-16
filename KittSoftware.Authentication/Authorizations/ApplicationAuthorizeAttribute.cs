// *****************************************************************************************************************
// Project          : Navyblue
// File             : ApplicationAuthorizeAttribute.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:53
// *****************************************************************************************************************
// <copyright file="ApplicationAuthorizeAttribute.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Authorization;
using System;

namespace Navyblue.Authentication.Filters
{
    /// <summary>
    ///     ApplicationAuthorizeAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApplicationAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ApplicationAuthorizeAttribute" /> class.
        /// </summary>
        public ApplicationAuthorizeAttribute()
        {
            this.Roles = "Application";
        }
    }
}