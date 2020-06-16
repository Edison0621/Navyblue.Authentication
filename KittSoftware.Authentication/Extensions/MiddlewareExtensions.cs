// *****************************************************************************************************************
// Project          : Navyblue
// File             : MiddlewareExtensions.cs
// Created          : 2019-01-14  17:44
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:54
// *****************************************************************************************************************
// <copyright file="MiddlewareExtensions.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Builder;
using Navyblue.Authentication.Middlewares.Middleware;

namespace Navyblue.Authentication.Extensions
{
    public static class MiddlewareExtensions
    {
        /// <summary>
        ///     Uses the jinyinmao json response wapper handler.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>HttpConfiguration.</returns>
        public static IApplicationBuilder UseJsonResponseWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JsonResponseWrapperMiddleware>();
        }

        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            //ExceptionHandlerExtensions
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        /// <summary>
        ///     Uses the trace entry.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseTraceEntry(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TraceEntryMiddleware>();
        }
    }
}