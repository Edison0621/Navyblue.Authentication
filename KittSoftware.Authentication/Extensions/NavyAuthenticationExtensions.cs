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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Navyblue.Authorizations.Authorizations.NavyblueResult;

namespace Navyblue.Authentication
{
    public static class NavyAuthenticationExtensions
    {
        public static IApplicationBuilder AddBearer(this IApplicationBuilder app)
        {
            app.UseAuthorization();
            app.UseAuthentication();

            return app;
        }

        public static IServiceCollection AddBearerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthorizationConfig>(configuration);

            services.AddAuthentication(AuthorizationScheme.Bearer)
                .AddScheme<BasicAuthenticationOptions, NavyAuthenticationHandler>(AuthorizationScheme.Bearer, null);

            services.AddMvc(p =>
            {
                p.Filters.Add(new NavyblueAuthorizationFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            });

            return services;
        }
    }
}