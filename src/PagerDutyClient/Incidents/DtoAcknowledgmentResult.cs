//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoAcknowledgmentResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to transfer the acknowledgment result object from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <inheritdoc cref="DtoBaseListResult"/>
/// <summary>
/// A class to transfer the acknowledgment result object from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoAcknowledgmentResult : DtoBaseListResult
{
    /// <summary>
    /// Gets or sets the acknowledger. The acknowledger represents the entity that made the acknowledgement for an incident.
    /// </summary>
    [JsonProperty("acknowledger")]
    public DtoBaseResult? Acknowledger { get; set; }
}
