//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoBodyResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A base class to transfer the body result objects from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A base class to transfer the body result objects from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoBodyResult
{
    /// <summary>
    /// Gets or sets the type. Allowed value: incident_body.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the details.
    /// </summary>
    [JsonProperty("details")]
    public string Details { get; set; } = string.Empty;
}
