// <copyright file="SendEmail.cs" company="Microsoft">
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
    public class SendEmail
    {
        /// <summary>Gets or sets the name of the event.</summary>
        /// <value>The name of the event.</value>
        public string EventName { get; set; }

        /// <summary>Gets or sets the type of the event.</summary>
        /// <value>The type of the event.</value>
        public string EventType { get; set; }

        /// <summary>Gets or sets the event subject.</summary>
        /// <value>The event subject.</value>
        public string EventSubject { get; set; }

        /// <summary>Gets or sets the name of the business process.</summary>
        /// <value>The name of the business process.</value>
        public string BusinessProcessName { get; set; }

        /// <summary>Gets or sets the properties.</summary>
        /// <value>The properties.</value>
        public IDictionary Properties { get; set; }

        /// <summary>Gets or sets the payload.</summary>
        /// <value>The payload.</value>
        public string Payload { get; set; }

        /// <summary>Gets or sets the publisher.</summary>
        /// <value>The publisher.</value>
        public IDictionary Publisher { get; set; }

        /// <summary>Gets or sets the event version.</summary>
        /// <value>The event version.</value>
        public string EventVersion { get; set; }

        /// <summary>Gets or sets the notification.</summary>
        /// <value>The notification.</value>
        public Notification Notification { get; set; }
    }
}