// ******************************************************************************************************
// Project          : Navyblue.Authorization
// File             : TokenGenerator.cs
// Created          : 2024-08-08  13:08
// 
// Last Modified By : jstsm(jstsmaxx@163.com)
// Last Modified On : 2024-08-08  13:08
// ******************************************************************************************************
// <copyright file="TokenGenerator.cs" company="">
//     Copyright ©  2011-2024. All rights reserved.
// </copyright>
// ******************************************************************************************************


using System.Security.Cryptography;
using System.Text;
using System;
using Navyblue.BaseLibrary;

namespace Navyblue.Authorization.Authorizations.Applications;

public static class TokenGenerator
{
    public static string GenerateInternalToken(string name, string role, string otherInfo, DateTime expires, RSA privateKey)
    {
        string ticket = $"{name},{role},{otherInfo},{new DateTimeOffset(expires).ToUnixTimeSeconds()}";
        byte[] ticketBytes = ticket.GetBytesOfASCII();
        byte[] signatureBytes = privateKey.SignData(ticketBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        string signature = Convert.ToBase64String(signatureBytes);
        string token = $"{ticket},{signature}";

        return Convert.ToBase64String(Encoding.ASCII.GetBytes(token));
    }
}