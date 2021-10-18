// <copyright file="EmailNotification.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace PSDIPortal.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    ///   <br />
    /// </summary>
    public class EmailNotification
    {
        /// <summary>Gets or sets a value indicating whether this <see cref="EmailNotification" /> is enabled.</summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>Gets or sets the channel.</summary>
        /// <value>The channel.</value>
        public string Channel { get; set; }

        /// <summary>Gets or sets the notifications.</summary>
        /// <value>The notifications.</value>
        public List<Dictionary<string, object>> Notifications { get; set; }
    }
}