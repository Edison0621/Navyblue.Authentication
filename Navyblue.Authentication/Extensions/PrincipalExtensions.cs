// *****************************************************************************************************************
// Project          : Navyblue
// File             : PrincipalExtensions.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:53
// *****************************************************************************************************************
// <copyright file="PrincipalExtensions.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using System.Security.Claims;
using System.Security.Principal;

namespace Navyblue.Authorization.Extensions;

/// <summary>
///     PrincipalExtensions.
/// </summary>
public static class PrincipalExtensions
{
    /// <summary>
    ///     Determines whether the specified principal is application.
    /// </summary>
    /// <param name="principal">The principal.</param>
    /// <returns><c>true</c> if the specified principal is application; otherwise, <c>false</c>.</returns>
    public static bool IsApplication(this IPrincipal principal)
    {
        return principal.IsRole("Application");
    }

    /// <summary>
    ///     Determines whether the specified principal is role.
    /// </summary>
    /// <param name="principal">The principal.</param>
    /// <param name="roleName">Name of the role.</param>
    /// <returns>System.Boolean.</returns>
    public static bool IsRole(this IPrincipal principal, string roleName)
    {
        ClaimsPrincipal claimsPrincipal = principal as ClaimsPrincipal;
        if (claimsPrincipal?.Identity is ClaimsIdentity claimsIdentity)
        {
            return claimsIdentity.HasClaim(ClaimTypes.Role, roleName);
        }

        return false;
    }

    /// <summary>
    ///     Determines whether the specified principal is user.
    /// </summary>
    /// <param name="principal">The principal.</param>
    /// <returns>System.Boolean.</returns>
    public static bool IsUser(this IPrincipal principal)
    {
        return principal.IsRole("User");
    }
}