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


namespace Navyblue.Authentication.Authorizations
{
    /// <summary>
    ///     AuthScheme.
    /// </summary>
    public static class AuthorizationScheme
    {
        /// <summary>
        ///     Basic
        /// </summary>
        public const string Basic = "Basic";

        /// <summary>
        ///     Bearer
        /// </summary>
        public const string Bearer = "Bearer";

        /// <summary>
        ///     The internal authentication
        /// </summary>
        public const string InternalAuth = "InternalAuth";

        /// <summary>
        ///     Quick
        /// </summary>
        public const string Quick = "Quick";

        /// <summary>
        ///     Wechat
        /// </summary>
        public const string Wechat = "Wechat";
    }
}