// *****************************************************************************************************************
// Project          : Navyblue
// File             : JsonRequestMessage.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:55
// *****************************************************************************************************************
// <copyright file="JsonRequestMessage.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

namespace Navyblue.Authentication
{
    /// <summary>
    ///     Class JsonRequestMessage.
    /// </summary>
    public class JsonRequestMessage
    {
        /// <summary>
        ///     Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>
        public string Method { get; set; }

        /// <summary>
        ///     Gets or sets the relative URL.
        /// </summary>
        /// <value>The relative URL.</value>
        public string RelativeUrl { get; set; }
    }
}