// *****************************************************************************************************************
// Project          : Navyblue
// File             : OrderedFilterProvider.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:54
// *****************************************************************************************************************
// <copyright file="OrderedFilterProvider.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Navyblue.Authorization.Filters
{
    /// <summary>
    ///     Class OrderedFilterProvider.
    /// </summary>
    public class OrderedFilterProvider : IFilterProvider
    {
        #region IFilterProvider Members

        public int Order => throw new NotImplementedException();

        public void OnProvidersExecuted(FilterProviderContext context)
        {
            throw new NotImplementedException();
        }

        public void OnProvidersExecuting(FilterProviderContext context)
        {
            throw new NotImplementedException();
        }

        #endregion IFilterProvider Members
    }
}