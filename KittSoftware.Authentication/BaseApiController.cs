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
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Navyblue.Authorization
{
    /// <summary>
    ///     BaseApiController.
    /// </summary>
    public abstract class BaseApiController : Controller
    {
        /// <summary>
        ///     Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public ITraceWriter Logger
        {
            get { return this.HttpContext.RequestServices.GetService<ITraceWriter>(); }
        }
    }
}