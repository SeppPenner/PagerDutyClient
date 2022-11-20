//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoResponderRequestTargetsResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A base class to transfer the responder request targets result objects from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A base class to transfer the responder request targets result objects from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoResponderRequestTargetsResult
{
    /// <summary>
    /// Gets or sets the type of the target (either a user or an escalation policy).
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the user or escalation policy.
    /// </summary>
    [JsonProperty("id")]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the summary.
    /// </summary>
    [JsonProperty("summary")]
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the array of responders associated with the specified incident.
    /// </summary>
    [JsonProperty("incident_responders")]
    public List<DtoIncidentRespondersResult> IncidentResponders { get; set; } = new();
}
