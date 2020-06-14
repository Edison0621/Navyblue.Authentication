// *****************************************************************************************************************
// Project          : Navyblue
// File             : NBInternalHttpClientFactory.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:55
// *****************************************************************************************************************
// <copyright file="NBInternalHttpClientFactory.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Http;
using Navyblue.Authentication.Extensions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Navyblue.BaseLibrary;

namespace Navyblue.Authentication
{
    /// <summary>
    ///     NBInternalHttpClientFactory.
    /// </summary>
    public static class InternalHttpClientFactory
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="T:System.Net.Http.HttpClient" />.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="traceEntry">The trace entry.</param>
        /// <param name="handlers">The list of HTTP handler that delegates the processing of HTTP response messages to another handler.</param>
        /// <returns>A new instance of the <see cref="T:System.Net.Http.HttpClient" />.</returns>
        public static HttpClient Create(string serviceName, TraceEntry traceEntry, params DelegatingHandler[] handlers)
        {
            //List<DelegatingHandler> delegatingHandlers = new List<DelegatingHandler>
            //{
            //    new NBTraceEntryHandler(traceEntry),
            //    new NBHttpStatusHandler(),
            //    new NBLogHandler("HTTP Client Request", "HTTP Client Response"),
            //    new NBRetryHandler()
            //};
            //delegatingHandlers.AddRange(handlers);

            //HttpClient client = HttpClientFactory.Create(new HttpClientHandler
            //{
            //    AllowAutoRedirect = true,
            //    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            //}, delegatingHandlers.ToArray());

            HttpClient client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json", 1.0));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.5));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.1));
            client.DefaultRequestHeaders.AcceptEncoding.Clear();
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip", 1.0));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate", 0.5));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("*", 0.1));
            client.Timeout = 1.Minutes();

            //KeyValuePair<string, string>? permission = App.Configurations.GetPermission(serviceName);
            //if (permission.HasValue)
            //{
            //    client.BaseAddress = new Uri(permission.Value.Key);
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthScheme.InternalAuth, permission.Value.Value);
            //}
            //else
            //{
            //    client.BaseAddress = new Uri("http://service.nb.com.cn/");
            //}

            return client;
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="T:System.Net.Http.HttpClient" />.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="traceEntry">The trace entry.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="handlers">The list of HTTP handler that delegates the processing of HTTP response messages to another handler.</param>
        /// <returns>A new instance of the <see cref="T:System.Net.Http.HttpClient" />.</returns>
        public static HttpClient Create(string serviceName, TraceEntry traceEntry, string userId, params DelegatingHandler[] handlers)
        {
            if (userId.IsNotNullOrEmpty())
            {
                traceEntry.UserId = userId;
            }

            return Create(serviceName, traceEntry, handlers);
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="T:System.Net.Http.HttpClient" />.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="request">The request.</param>
        /// <param name="handlers">The list of HTTP handler that delegates the processing of HTTP response messages to another handler.</param>
        /// <returns>A new instance of the <see cref="T:System.Net.Http.HttpClient" />.</returns>
        public static HttpClient Create(string serviceName, HttpRequest request, params DelegatingHandler[] handlers)
        {
            return Create(serviceName, request, "", handlers);
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="T:System.Net.Http.HttpClient" />.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="request">The request.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="handlers">The list of HTTP handler that delegates the processing of HTTP response messages to another handler.</param>
        /// <returns>A new instance of the <see cref="T:System.Net.Http.HttpClient" />.</returns>
        public static HttpClient Create(string serviceName, HttpRequest request, string userId, params DelegatingHandler[] handlers)
        {
            return Create(serviceName, request.GetTraceEntry(), userId, handlers);
        }
    }
}