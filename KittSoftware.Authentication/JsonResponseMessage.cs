// *****************************************************************************************************************
// Project          : Navyblue
// File             : JsonResponseMessage.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:55
// *****************************************************************************************************************
// <copyright file="JsonResponseMessage.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using System.Collections.Generic;

namespace Navyblue.Authorization
{
    /// <summary>
    ///     Class JsonResponseMessage.
    /// </summary>
    public class JsonResponseMessage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="JsonResponseMessage" /> class.
        /// </summary>
        public JsonResponseMessage()
        {
            this.Headers = new Dictionary<string, string>();
        }

        /// <summary>
        ///     Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public int Code { get; set; }

        /// <summary>
        ///     Gets or sets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public Dictionary<string, string> Headers { get; set; }
    }
}