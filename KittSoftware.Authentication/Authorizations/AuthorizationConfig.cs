// ******************************************************************************************************
// Project          : AuthTest
// File             : AuthConfig.cs
// Created          : 2020-06-14  20:18
// 
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2020-06-14  20:18
// ******************************************************************************************************
// <copyright file="AuthConfig.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2020 Shanghai Future  Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************


using System.Collections.Generic;

namespace Navyblue.Authorization.Authorizations;

public class AuthorizationConfig
{
    public bool UseSwaggerAsApplicationForDev { get; set; }

    public List<string> AllowedLists { get; set; }

    public string GovernmentServerPublicKey { get; set; }

    public string BearerAuthKeys { get; set; }

    public int PcSignInExpirationSeconds { get; set; }

    public int AppSignInExpirationSeconds { get; set; }

    public int MobileSignInExpirationSeconds { get; set; }

    public string AndroidClientId { get; set; }

    public string IosClientId { get; set; }

    public string AuthHeaderName { get; set; }
}