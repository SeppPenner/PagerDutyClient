//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoPendingActionResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to transfer the pending action result object from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <inheritdoc cref="DtoBaseListResult"/>
/// <summary>
/// A class to transfer the pending action result object from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoPendingActionResult : DtoBaseListResult
{
    /// <summary>
    /// Gets or sets the type. Allowed values: unacknowledge escalate resolve urgency_change.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;
}
