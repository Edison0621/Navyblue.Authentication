// ******************************************************************************************************
// Project          : Navyblue.Authorization
// File             : InternalHttpClientFactory.cs
// Created          : 2024-08-08  13:08
// 
// Last Modified By : jstsm(jstsmaxx@163.com)
// Last Modified On : 2024-08-08  13:08
// ******************************************************************************************************
// <copyright file="InternalHttpClientFactory.cs" company="">
//     Copyright ©  2011-2024. All rights reserved.
// </copyright>
// ******************************************************************************************************


using System.Net.Http;
using System.Security.Cryptography;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Navyblue.Authorization.Authorizations.Applications;

public static class InternalHttpClientFactoryExtension
{
    public static HttpClient CreateInternalClient(this IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider, string applicationName, string otherInfo = "")
    {
        RSA rsa = serviceProvider.GetRequiredService<RSA>();
        HttpClient client = httpClientFactory.CreateClient(applicationName);

        client.DefaultRequestHeaders.Add(AuthorizationHeaderName.ApplicationName, $"{AuthorizationScheme.INTERNAL_AUTH} {TokenGenerator.GenerateInternalToken(applicationName, "Application", otherInfo, DateTime.UtcNow.AddHours(1), rsa)}");
        return client;
    }
}