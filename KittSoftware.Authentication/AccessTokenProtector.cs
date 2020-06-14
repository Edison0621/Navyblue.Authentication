// *****************************************************************************************************************
// Project          : Navyblue
// File             : NBAccessTokenProtector.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:55
// *****************************************************************************************************************
// <copyright file="NBAccessTokenProtector.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using Navyblue.BaseLibrary;

namespace Navyblue.Authentication
{
    /// <summary>
    ///     NBAccessTokenProtector.
    /// </summary>
    public sealed class AccessTokenProtector
    {
        private const string Anonymous = "Anonymous";
        private const string CRYPTO_SERVICE_PROVIDER_ERROR_MESSAGE = "AccessTokenProtector RSACryptoServiceProvider can not initialize. The key may be in bad format. Key: {0}";
        private const string Unspecified = "Unspecified";

        /// <summary>
        ///     The RSA crypto service provider
        /// </summary>
        private readonly RSACryptoServiceProvider rsaCryptoServiceProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessTokenProtector" /> class.
        /// </summary>
        /// <param name="key">The cryptographic key.</param>
        public AccessTokenProtector(string key)
        {
            try
            {
                this.rsaCryptoServiceProvider = new RSACryptoServiceProvider();
                this.rsaCryptoServiceProvider.FromXmlString(key);
            }
            catch (Exception e)
            {
                throw new Exception(CRYPTO_SERVICE_PROVIDER_ERROR_MESSAGE.FormatWith(key), e);
            }
        }

        /// <summary>
        ///     Protects the specified identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns>System.String.</returns>
        public string Protect(ClaimsIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            string name = identity.Name ?? Anonymous;
            Claim claim = identity.FindFirst(ClaimTypes.Expiration);
            long timestamp = claim?.Value?.AsLong() ?? DateTime.UtcNow.UnixTimestamp();
            string scheme = identity.AuthenticationType ?? Unspecified;
            string payload = $"{name},{timestamp},{scheme}";

            return this.rsaCryptoServiceProvider.Encrypt(payload.GetBytesOfASCII(), false).ToBase64String();
        }

        /// <summary>
        ///     Unprotects the specified protected data.
        /// </summary>
        /// <param name="protectedData">The protected data.</param>
        /// <returns>System.Security.Claims.ClaimsIdentity.</returns>
        public ClaimsIdentity Unprotect(string protectedData)
        {
            if (protectedData == null)
            {
                throw new ArgumentNullException(nameof(protectedData));
            }

            List<Claim> claims = new List<Claim>();

            try
            {
                byte[] unprotectedData = this.rsaCryptoServiceProvider.Decrypt(protectedData.ToBase64Bytes(), false);
                string identityData = unprotectedData.ASCII();
                string[] identityDatas = identityData.Split(',');
                long timestamp = identityDatas[1]?.AsLong() ?? 0L;
                if (timestamp < DateTime.UtcNow.UnixTimestamp())
                {
                    claims.Add(new Claim(ClaimTypes.Expired, "True"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Name, identityDatas[0] ?? Anonymous));
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                    return new ClaimsIdentity(claims, identityDatas[2] ?? Unspecified);
                }
            }
            catch (Exception e)
            {
                claims.Add(new Claim(ClaimTypes.AuthorizationDecision, "Error:" + e.Message));
            }

            return new ClaimsIdentity(claims);
        }
    }
}