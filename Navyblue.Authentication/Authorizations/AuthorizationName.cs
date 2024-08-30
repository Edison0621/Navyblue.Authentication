// ******************************************************************************************************
// Project          : Navyblue.Authorization
// File             : AuthorizationName.cs
// Created          : 2024-08-08  14:08
// 
// Last Modified By : jstsm(jstsmaxx@163.com)
// Last Modified On : 2024-08-08  14:08
// ******************************************************************************************************
// <copyright file="AuthorizationName.cs" company="">
//     Copyright ©  2011-2024. All rights reserved.
// </copyright>
// ******************************************************************************************************

using System.Collections.Generic;

namespace Navyblue.Authorization.Authorizations;

/// <summary>
/// 
/// </summary>
public static class AuthorizationHeaderName
{
    /// <summary>
    /// The application name
    /// </summary>
    public static readonly string ApplicationName = "App-Authorization";

    public static readonly Dictionary<string, string> AuthorizationHeaderNames = new()
    {
        { "ApplicationName", "App-Authorization" }
    };
}