//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoIncidentDataRequest.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to transfer the incidents request data from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A class to transfer the incidents request data from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
internal class DtoIncidentDataRequest
{
    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resolution. (Only set for resolved status).
    /// </summary>
    [JsonProperty("resolution")]
    public string? Resolution { get; set; }
}