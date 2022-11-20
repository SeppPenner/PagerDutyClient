//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoResolveReasonResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to transfer the resolve reason result object from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A class to transfer the resolve reason result object from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoResolveReasonResult
{
    /// <summary>
    /// Gets or sets the type. The reason the incident was resolved. The only reason currently supported is merge.
    /// Allowed value: merge_resolve_reason.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the incident.
    /// </summary>
    [JsonProperty("incident")]
    public DtoBaseResult? Incident { get; set; }
}
