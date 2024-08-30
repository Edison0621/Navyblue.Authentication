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


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Navyblue.Authorization.Authorizations;
using Navyblue.Authorization.Authorizations.NavyblueResult;
using System.Security.Cryptography;
using Navyblue.BaseLibrary;

namespace Navyblue.Authorization.Extensions;

public static class AuthenticationExtensions
{
    public static IApplicationBuilder UseBearer(this IApplicationBuilder app)
    {
        app.UseAuthorization();
        app.UseAuthentication();

        return app;
    }

    public static IServiceCollection AddBearer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthorizationConfig>(configuration);

        services.AddSingleton(provider =>
        {
            RSA rsa = RSA.Create();
            rsa.FromXmlString(configuration["InternalPrivateKey"]!.HtmlDecode());
            return rsa;
        });

        services.AddAuthentication(AuthorizationScheme.BEARER)
            .AddScheme<BasicAuthenticationOptions, AuthenticationHandler>(AuthorizationScheme.BEARER, null);

        services.AddAuthentication(AuthorizationScheme.INTERNAL_AUTH)
            .AddScheme<BasicAuthenticationOptions, AuthenticationHandler>(AuthorizationScheme.INTERNAL_AUTH, null);

        services.AddControllers(p => p.Filters.Add(new AuthorizationFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())));

        return services;
    }
}