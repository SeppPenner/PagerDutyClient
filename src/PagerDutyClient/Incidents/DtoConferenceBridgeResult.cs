//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoConferenceBridgeResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A base class to transfer the conference bridge result objects from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A base class to transfer the conference bridge result objects from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoConferenceBridgeResult
{
    /// <summary>
    /// Gets or sets the conference number.
    /// </summary>
    [JsonProperty("conference_number")]
    public string ConferenceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the conference Url.
    /// </summary>
    [JsonProperty("conference_url")]
    public string ConferenceUrl { get; set; } = string.Empty;
}
