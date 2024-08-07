﻿// *****************************************************************************************************************
// Project          : Navyblue
// File             : UserAuthorizeAttribute.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:54
// *****************************************************************************************************************
// <copyright file="UserAuthorizeAttribute.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Authorization;

namespace Navyblue.Authorization.Authorizations;

/// <summary>
///     UserAuthorizeAttribute.
/// </summary>
public class UserAuthorizeAttribute : AuthorizeAttribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserAuthorizeAttribute" /> class.
    /// </summary>
    public UserAuthorizeAttribute():this(AuthorizationScheme.BEARER)
    {
        this.Roles = AuthorizationRole.User;
        this.AuthenticationSchemes = AuthorizationScheme.BEARER;
    }

    private UserAuthorizeAttribute(string schemes)
    {

    }
}