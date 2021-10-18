// <copyright file="Notification.cs" company="Microsoft">
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
    public class Notification
    {
        /// <summary>Gets or sets the email notification.</summary>
        /// <value>The email notification.</value>
        public EmailNotification EmailNotification { get; set; }
    }
}