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
using Navyblue.Authentication.Authorizations;
using Navyblue.Authorization.Authorizations.NavyblueResult;

namespace Navyblue.Authentication.Extensions
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

            services.AddMvc(p => p.Filters.Add(new NavyblueAuthorizationFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())));

            return services;
        }
    }
}