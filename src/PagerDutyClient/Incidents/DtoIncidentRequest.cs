//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoIncidentRequest.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to transfer the incidents request from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A class to transfer the incidents request from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
internal class DtoIncidentRequest
{
    /// <summary>
    /// Gets or sets the incident.
    /// </summary>
    [JsonProperty("incident")]
    public DtoIncidentDataRequest Incident { get; set; } = new();
}