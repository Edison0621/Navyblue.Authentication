// *****************************************************************************************************************
// Project          : Navyblue
// File             : INavyBlueMiddleware.cs
// Created          : 2019-01-14  17:44
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:54
// *****************************************************************************************************************
// <copyright file="INavyBlueMiddleware.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Navyblue.Authentication.Middlewares.Middleware
{
    public interface INavyBlueMiddleware
    {
        Task Invoke(HttpContext context);
    }
}