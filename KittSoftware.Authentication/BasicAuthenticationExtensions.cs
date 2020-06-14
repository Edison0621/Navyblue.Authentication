// ******************************************************************************************************
// Project          : AuthTest
// File             : BasicAuthenticationExtensions.cs
// Created          : 2020-06-14  22:55
// 
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2020-06-14  22:55
// ******************************************************************************************************
// <copyright file="BasicAuthenticationExtensions.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2020 Shanghai Future  Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************


using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Navyblue.Authentication
{
    public static class BasicAuthenticationExtensions
    {
        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme)
        {
            return AddBasic(builder, authenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder, string authenticationScheme, Action<BasicAuthenticationOptions> configureOptions)
        {
            builder.Services.AddSingleton<IPostConfigureOptions<BasicAuthenticationOptions>, BasicAuthenticationPostConfigureOptions>();

            return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(
                authenticationScheme, configureOptions);
        }
    }
}