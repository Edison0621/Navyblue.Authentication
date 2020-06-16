// ******************************************************************************************************
// Project          : AuthTest
// File             : TraceEntry.cs
// Created          : 2020-06-14  16:14
// 
// Last Modified By : (jstsmaxx@163.com)
// Last Modified On : 2020-06-14  16:14
// ******************************************************************************************************
// <copyright file="TraceEntry.cs" company="Shanghai Future Mdt InfoTech Ltd.">
//     Copyright ©  2012-2020 Shanghai Future  Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ******************************************************************************************************


using System;

namespace Navyblue.Authorization
{
    /// <summary>
    ///     TraceEntry.
    /// </summary>
    public class TraceEntry : IEquatable<TraceEntry>
    {
        /// <summary>
        ///     Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public string ClientId { get; set; }

        /// <summary>
        ///     Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public string DeviceId { get; set; }

        /// <summary>
        ///     Gets or sets the request identifier.
        /// </summary>
        /// <value>The request identifier.</value>
        public string RequestId { get; set; }

        /// <summary>
        ///     Gets or sets the session identifier.
        /// </summary>
        /// <value>The session identifier.</value>
        public string SessionId { get; set; }

        /// <summary>
        ///     Gets or sets the source ip.
        /// </summary>
        /// <value>The source ip.</value>
        public string SourceIP { get; set; }

        /// <summary>
        ///     Gets or sets the source user agent.
        /// </summary>
        /// <value>The source user agent.</value>
        public string SourceUserAgent { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }

        #region IEquatable<TraceEntry> Members

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(TraceEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(this.ClientId, other.ClientId) && string.Equals(this.DeviceId, other.DeviceId) && string.Equals(this.RequestId, other.RequestId) && string.Equals(this.SessionId, other.SessionId) && string.Equals(this.SourceIP, other.SourceIP) && string.Equals(this.SourceUserAgent, other.SourceUserAgent) && string.Equals(this.UserId, other.UserId);
        }

        #endregion IEquatable<TraceEntry> Members

        /// <summary>
        ///     Implements the !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TraceEntry left, TraceEntry right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Implements the ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(TraceEntry left, TraceEntry right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((TraceEntry)obj);
        }

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (this.ClientId != null ? this.ClientId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.DeviceId != null ? this.DeviceId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.RequestId != null ? this.RequestId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SessionId != null ? this.SessionId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SourceIP != null ? this.SourceIP.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SourceUserAgent != null ? this.SourceUserAgent.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.UserId != null ? this.UserId.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}