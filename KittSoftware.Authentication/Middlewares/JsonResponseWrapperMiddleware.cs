// *****************************************************************************************************************
// Project          : Navyblue
// File             : JsonResponseWarpperMiddleware.cs
// Created          : 2019-01-14  17:44
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:54
// *****************************************************************************************************************
// <copyright file="JsonResponseWarpperMiddleware.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Navyblue.BaseLibrary;
using Navyblue.Authorization;

namespace Navyblue.Authentication.Middlewares
{
    public class JsonResponseWrapperMiddleware : INavyBlueMiddleware
    {
        private readonly RequestDelegate _next;

        public JsonResponseWrapperMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        #region INavyBlueMiddleware Members

        public async Task Invoke(HttpContext context)
        {
            await this._next.Invoke(context);

            if (context.Request.Headers[HeaderNames.Accept].ToString() == "application/json"
                && context.Response != null
                && context.Response.Headers[HeaderNames.ContentType].ToString().Contains("application/json"))
            {
                await this.WrapperUpContent(context);
            }
        }

        #endregion INavyBlueMiddleware Members

        private async Task WrapperUpContent(HttpContext context)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Stream orgBodyStream = context.Response.Body;
                context.Response.Body = ms;

                using (StreamReader sr = new StreamReader(ms))
                {
                    ms.Seek(0, SeekOrigin.Begin);

                    object data = sr.ReadToEnd().FromJson<object>();
                    JsonResponseMessage resultData = new JsonResponseMessage
                    {
                        Code = context.Response.StatusCode,
                        Body = data.ToJson()
                    };
                    string toJson = JsonConvert.SerializeObject(resultData);
                    byte[] theBan = toJson.GetBytesOfUTF8(); 

                    ms.SetLength(0);
                    ms.Write(theBan);

                    ms.Seek(0, SeekOrigin.Begin);
                    await ms.CopyToAsync(orgBodyStream);
                    context.Response.Body = orgBodyStream;
                }
            }
        }
    }
}