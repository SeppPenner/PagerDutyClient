﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoPagerDutyPayload.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The DTO class to transfer payloads for PagerDuty.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient
{
    using System;

    using Newtonsoft.Json;

    using NJsonSchema.Annotations;

    /// <summary>
    /// The DTO class to transfer payloads for PagerDuty.
    /// </summary>
    [JsonSchemaFlatten]
    [Serializable]
    public class DtoPagerDutyPayload
    {
        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        [JsonProperty("summary")]
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        [JsonProperty("severity")]
        public string Severity { get; set; }
    }
}