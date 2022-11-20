// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoEventPayload.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The DTO class to transfer payloads for the PagerDuty events API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Events;

/// <summary>
/// The DTO class to transfer payloads for the PagerDuty events API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
internal class DtoEventPayload
{
    /// <summary>
    /// Gets or sets the summary.
    /// </summary>
    [JsonProperty("summary")]
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    [JsonProperty("source")]
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the severity.
    /// </summary>
    [JsonProperty("severity")]
    public string Severity { get; set; } = string.Empty;
}