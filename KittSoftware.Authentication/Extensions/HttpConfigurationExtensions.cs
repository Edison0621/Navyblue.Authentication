// *****************************************************************************************************************
// Project          : Navyblue
// File             : HttpConfigurationExtensions.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-14  17:23
// *****************************************************************************************************************
// <copyright file="HttpConfigurationExtensions.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Mvc.Filters;
using NavyBlue.AspNetCore.Web.Diagnostics;
using NavyBlue.AspNetCore.Web.Filters;
using NavyBlue.AspNetCore.Web.Handlers;
using NavyBlue.AspNetCore.Web.Handlers.Server;
using Newtonsoft.Json.Serialization;

namespace NavyBlue.AspNetCore.Web.Extensions
{
    /// <summary>
    ///     HttpConfigurationExtensions.
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        ///     Maps the HTTP batch route.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration MapHttpBatchRoute(this HttpConfiguration config)
        {
            config.Routes.MapHttpBatchRoute("WebApiBatch", "$batch", new BatchHandler(GlobalConfiguration.DefaultServer));
            return config;
        }

        /// <summary>
        ///     Uses the UseNBAuthorizationHandler.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="bearerAuthKeys">The bearer keys.</param>
        /// <param name="governmentServerPublicKey">The government server public key.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNBAuthorizationHandler(this HttpConfiguration config, string bearerAuthKeys, string governmentServerPublicKey)
        {
            config.MessageHandlers.Add(new NBAuthorizationHandler(bearerAuthKeys, governmentServerPublicKey));
            return config;
        }

        /// <summary>
        ///     Uses the UseNBExceptionHandler.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNBExceptionHandler(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), new NBExceptionHandler());
            return config;
        }

        /// <summary>
        ///     Uses the UseNBExceptionLogger.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNBExceptionLogger(this HttpConfiguration config)
        {
            config.Services.Add(typeof(IExceptionLogger), new NBExceptionLogger());
            return config;
        }

        /// <summary>
        ///     Uses the UseNBJsonResponseWapperHandler.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNBJsonResponseWapperHandler(this HttpConfiguration config)
        {
            config.MessageHandlers.Add(new NBJsonResponseWarpperHandler());
            return config;
        }

        /// <summary>
        ///     Uses the UseNBLogger.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNBLogger(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(ITraceWriter), new NBTraceWriter());
            config.Services.Add(typeof(IExceptionLogger), new NBExceptionLogger());
            return config;
        }

        /// <summary>
        ///     Uses the UseNBLogHandler.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNBLogHandler(this HttpConfiguration config)
        {
            config.MessageHandlers.Add(new NBLogHandler());
            return config;
        }

        /// <summary>
        ///     Uses the UseNBLogHandler.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="requestTag">The request tag.</param>
        /// <param name="responseTag">The response tag.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNBLogHandler(this HttpConfiguration config, string requestTag, string responseTag)
        {
            config.MessageHandlers.Add(new NBLogHandler(requestTag, responseTag));
            return config;
        }

        /// <summary>
        ///     Uses the UseNBTraceEntryHandler.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNBTraceEntryHandler(this HttpConfiguration config)
        {
            config.MessageHandlers.Add(new NBTraceEntryHandler());
            return config;
        }

        /// <summary>
        ///     Uses the UseNBTraceWriter.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNBTraceWriter(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(ITraceWriter), new NBTraceWriter());
            return config;
        }

        /// <summary>
        ///     Uses the ordered filter.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseOrderedFilter(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(IFilterProvider), new ConfigurationFilterProvider());
            config.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());
            return config;
        }
    }
}