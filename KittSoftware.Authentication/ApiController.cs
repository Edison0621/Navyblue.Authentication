// *****************************************************************************************************************
// Project          : Navyblue
// File             : NBApiController.cs
// Created          : 2019-01-14  17:14
//
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2019-01-15  10:55
// *****************************************************************************************************************
// <copyright file="NBApiController.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2019 Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// *****************************************************************************************************************

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Navyblue.Authorization
{
    /// <summary>
    ///     ApiController.
    /// </summary>
    [ApiController]
    public abstract class ApiController: Controller
    {
        /// <summary>
        ///     Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public ITraceWriter Logger
        {
            get { return this.HttpContext.RequestServices.GetService<ITraceWriter>(); }
        }

        /// <summary>
        ///     Provides a set of methods and properties that help debug your code with the specified writer, request, exception, message format and argument.
        /// </summary>
        /// <param name="message">The log message.</param>
        protected void Debug(string message)
        {
            message = message.Replace("{", "{{").Replace("}", "}}");
            this.Logger.Trace(TraceLevel.Verbose, "Application:" + message, null);
        }

        /// <summary>
        ///     Provides a set of methods and properties that help debug your code with the specified writer, request, and exception.
        /// </summary>
        /// <param name="exception">The error occurred during execution.</param>
        protected void Debug(Exception exception)
        {
            this.Logger.Trace(TraceLevel.Verbose, "Application", exception);
        }

        /// <summary>
        ///     Provides a set of methods and properties that help debug your code with the specified writer, request, exception, message format and argument.
        /// </summary>
        /// <param name="exception">The error occurred during execution.</param>
        /// <param name="message">The log message.</param>
        protected void Debug(Exception exception, string message)
        {
            message = message.Replace("{", "{{").Replace("}", "}}");
            this.Logger.Trace(TraceLevel.Verbose, "Application:" + message, exception);
        }

        /// <summary>
        ///     Displays an error message in the list with the specified writer, request, message format and argument.
        /// </summary>
        /// <param name="message">The log message.</param>
        protected void Error(string message)
        {
            message = message.Replace("{", "{{").Replace("}", "}}");
            this.Logger.Trace(TraceLevel.Error, "Application:" + message, null);
        }

        /// <summary>
        ///     Displays an error message in the list with the specified writer, request, and exception.
        /// </summary>
        /// <param name="exception">The error occurred during execution.</param>
        protected void Error(Exception exception)
        {
            this.Logger.Trace(TraceLevel.Error, "Application", exception);
        }

        /// <summary>
        ///     Displays an error message in the list with the specified writer, request, exception, message format and argument.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The log message.</param>
        protected void Error(Exception exception, string message)
        {
            message = message.Replace("{", "{{").Replace("}", "}}");
            this.Logger.Trace(TraceLevel.Error, "Application:" + message, exception);
        }

        /// <summary>
        ///     Displays the details in the 
        /// </summary>
        /// <param name="message">The log message.</param>
        protected void Info(string message)
        {
            message = message.Replace("{", "{{").Replace("}", "}}");
            this.Logger.Trace(TraceLevel.Info, "Application:" + message, null);
        }

        /// <summary>
        ///     Displays the details in the 
        /// </summary>
        /// <param name="exception">The error occurred during execution.</param>
        protected void Info(Exception exception)
        {
            this.Logger.Trace(TraceLevel.Info, "Application", exception);
        }

        /// <summary>
        ///     Displays the details in the
        /// </summary>
        /// <param name="exception">The error occurred during execution.</param>
        /// <param name="message">The log message.</param>
        protected void Info(Exception exception, string message)
        {
            message = message.Replace("{", "{{").Replace("}", "}}");
            this.Logger.Trace(TraceLevel.Info, "Application:" + message, exception);
        }

        /// <summary>
        ///     Indicates the warning level of execution.
        /// </summary>
        /// <param name="message">The log message.</param>
        protected void Warn(string message)
        {
            message = message.Replace("{", "{{").Replace("}", "}}");
            this.Logger.Trace(TraceLevel.Warning, "Application:" + message, null);
        }

        /// <summary>
        ///     Indicates the warning level of execution.
        /// </summary>
        /// <param name="exception">The error occurred during execution.</param>
        protected void Warn(Exception exception)
        {
            this.Logger.Trace(TraceLevel.Warning, "Application", exception);
        }

        /// <summary>
        ///     Indicates the warning level of execution.
        /// </summary>
        /// <param name="exception">The error occurred during execution.</param>
        /// <param name="message">The log message.</param>
        protected void Warn(Exception exception, string message)
        {
            message = message.Replace("{", "{{").Replace("}", "}}");
            this.Logger.Trace(TraceLevel.Warning, "Application:" + message, exception);
        }
    }
}