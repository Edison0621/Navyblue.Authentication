// ******************************************************************************************************
// Project          : AuthTest
// File             : AuthScheme.cs
// Created          : 2020-06-14  20:01
// 
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2020-06-14  20:01
// ******************************************************************************************************
// <copyright file="AuthScheme.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2020 Shanghai Future  Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************


namespace Navyblue.Authorization.Authorizations;

/// <summary>
///     AuthScheme.
/// </summary>
public static class AuthorizationScheme
{
    /// <summary>
    ///     Basic
    /// </summary>
    public const string BASIC = "Basic";

    /// <summary>
    ///     Bearer
    /// </summary>
    public const string BEARER = "Bearer";

    /// <summary>
    ///     The internal authentication
    /// </summary>
    public const string INTERNAL_AUTH = "InternalAuth";

    /// <summary>
    ///     Quick
    /// </summary>
    public const string QUICK = "Quick";

    /// <summary>
    ///     Wechat
    /// </summary>
    public const string WECHAT = "Wechat";
}