// *****************************************************************************************************************
// Project          : Navyblue
// File             : HttpRequestExtensions.cs
// Created          : 2019-01-14  17:59
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:53
// *****************************************************************************************************************
// <copyright file="HttpRequestExtensions.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using Navyblue.BaseLibrary;
using ReflectionMagic;

namespace Navyblue.Authorization.Extensions;

public static class HttpRequestExtensions
{
    public static string GetHeader(this HttpRequest request, string headerName)
    {
        return request.Headers[headerName].ToString();
    }

    /// <summary>
    ///     Gets the trace entry.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Diagnostics.TraceEntry.</returns>
    public static TraceEntry GetTraceEntry(this HttpRequest request)
    {
        return request.To(r => new TraceEntry
        {
            ClientId = request?.GetHeader("X-NB-CID"),
            DeviceId = request?.GetHeader("X-NB-DID"),
            RequestId = request?.GetHeader("X-NB-RID"),
            SessionId = request?.GetHeader("X-NB-SID"),
            SourceIP = request?.GetHeader("X-NB-IP") ?? request?.GetHeader(HeaderNames.Host),
            SourceUserAgent = request?.GetHeader("X-NB-UA") ?? request?.GetHeader(HeaderNames.UserAgent),
            UserId = request?.GetHeader("X-NB-UID")
        });
    }

    /// <summary>
    ///     Returns a dictionary of QueryStrings that's easier to work with
    ///     than GetQueryNameValuePairs KevValuePairs collection.
    ///     If you need to pull a few single values use GetQueryString instead.
    /// </summary>
    /// <param name="request">The instance of <see cref="HttpRequest" />.</param>
    /// <returns>The QueryStrings dictionary.</returns>
    /// <exception cref="System.ArgumentNullException">If the request is null, throw the ArgumentNullException.</exception>
    public static Dictionary<string, string> GetQueryStrings(this HttpRequest request)
    {
        IQueryCollection queryStrings = request.Query;

        return queryStrings.ToDictionary(p=>p.Key,p=>p.Value.ToString());
    }

    /// <summary>
    ///     Returns the user host(ip) string value.
    /// </summary>
    /// <param name="request">The instance of <see cref="HttpRequestMessage" />.</param>
    /// <exception cref="System.ArgumentNullException">If the request is null, throw the ArgumentNullException.</exception>
    public static string GetUserHostAddress(HttpRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return request.Host.Host;
    }

    /// <summary>
    ///     Determines whether the specified HTTP httpContext is from dev.
    /// </summary>
    /// <param name="httpContext">The HTTP httpContext.</param>
    /// <param name="ipStartWith">The ip start with.</param>
    /// <returns><c>true</c> if the specified HTTP httpContext is dev; otherwise, <c>false</c>.</returns>
    public static bool IsFrom(HttpContext httpContext, string ipStartWith)
    {
        string ip = httpContext.Request.Host.Host;

        return !string.IsNullOrEmpty(ip) && ip.StartsWith(ipStartWith, StringComparison.Ordinal);
    }

    /// <summary>
    ///     Determines whether the specified request is from ios.
    /// </summary>
    /// <param name="httpContext">The HTTP httpContext.</param>
    /// <returns><c>true</c> if the specified request is ios; otherwise, <c>false</c>.</returns>
    public static bool IsFromIos(HttpContext httpContext)
    {
        string userAgent = GetUserAgent(httpContext);
        return userAgent != null && (userAgent.ToUpperInvariant().Contains("IPHONE") || userAgent.ToUpperInvariant().Contains("IPAD") || userAgent.ToUpperInvariant().Contains("IPOD"));
    }

    /// <summary>
    ///     Determines whether the specified HTTP httpContext is from localhost.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns><c>true</c> if the specified HTTP httpContext is localhost; otherwise, <c>false</c>.</returns>
    public static bool? IsFromLocalhost(HttpContext httpContext)
    {
        return httpContext.Connection.LocalIpAddress?.MapToIPv6().IsIPv6SiteLocal;
    }

    /// <summary>
    ///     Determines whether the specified request is from mobile device.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns><c>true</c> if the specified request is from mobile device; otherwise, <c>false</c>.</returns>
    public static bool IsFromMobileDevice(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            return false;
        }

        string userAgent = GetUserAgent(httpContext);
        if (userAgent.IsNullOrEmpty())
        {
            return false;
        }

        userAgent = userAgent.ToUpperInvariant();

        return userAgent.Contains("IPHONE") || userAgent.Contains("IOS") || userAgent.Contains("IPAD")
               || userAgent.Contains("ANDROID");
    }

    /// <summary>
    ///     Returns the user agent string value.
    /// </summary>
    /// <param name="httpContext">The instance of <see cref="HttpContext" />.</param>
    /// <returns>The user agent string value.</returns>
    public static string GetUserAgent(HttpContext httpContext)
    {
        return httpContext.Request.Headers[HeaderNames.UserAgent].ToString();
    }

    /// <summary>
    ///     Clones an <see cref="HttpWebRequest" /> in order to send it again.
    /// </summary>
    /// <param name="message">The message to set headers on.</param>
    /// <param name="request">The request with headers to clone.</param>
    public static void CopyHeadersFrom(this HttpRequestMessage message, HttpRequest request)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        foreach (string headerName in request.Headers.Keys)
        {
            string[] headerValues = request.Headers[headerName];
            if (!message.Headers.TryAddWithoutValidation(headerName, headerValues))
            {
                message.Content?.Headers.TryAddWithoutValidation(headerName, headerValues);
            }
        }
    }

    /// <summary>
    ///     Dumps the specified request include headers.
    /// </summary>
    /// <param name="httpRequest">The HTTP request.</param>
    /// <param name="includeHeaders">if set to <c>true</c> [include headers].</param>
    /// <returns>System.String.</returns>
    public static string Dump(this HttpRequest httpRequest, bool includeHeaders = true)
    {
        MemoryStream memoryStream = new();

        try
        {
            TextWriter writer = new StreamWriter(memoryStream);

            writer.Write(httpRequest.Method);
            writer.Write(httpRequest.HttpContext.Request.GetDisplayUrl());

            // headers

            if (includeHeaders)
            {
                if (httpRequest.AsDynamic()._wr != null)
                {
                    // real request -- add protocol
                    writer.Write(" " + httpRequest.AsDynamic()._wr.GetHttpVersion() + "\r\n");

                    // headers
                    writer.Write(httpRequest.AsDynamic().CombineAllHeaders(true));
                }
                else
                {
                    // manufactured request
                    writer.Write("\r\n");
                }
            }

            writer.Write("\r\n");
            writer.Flush();

            // entity body

            dynamic httpInputStream = httpRequest.AsDynamic().InputStream;
            httpInputStream.WriteTo(memoryStream);

            StreamReader reader = new(memoryStream);
            return reader.ReadToEnd();
        }
        finally
        {
            memoryStream.Close();
        }
    }
}