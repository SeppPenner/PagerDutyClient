//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoAlertCountResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A base class to transfer the alert counts result objects from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A base class to transfer the alert counts result objects from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoAlertCountResult
{
    /// <summary>
    /// Gets or sets the count of triggered alerts.
    /// </summary>
    [JsonProperty("triggered")]
    public int TriggeredAlertCount { get; set; }

    /// <summary>
    /// Gets or sets the count of resolved alerts.
    /// </summary>
    [JsonProperty("resolved")]
    public int ResolvedAlertCount { get; set; }

    /// <summary>
    /// Gets or sets the count of all alerts.
    /// </summary>
    [JsonProperty("all")]
    public int AllAlertCount { get; set; }
}
