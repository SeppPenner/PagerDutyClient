// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoPagerDuty.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The DTO class to transfer the data for PagerDuty.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient;

/// <summary>
/// The DTO class to transfer the data for PagerDuty.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoPagerDuty
{
    /// <summary>
    /// Gets or sets the routing key.
    /// </summary>
    [JsonProperty("routing_key")]
    public string Routing_Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event action.
    /// </summary>
    [JsonProperty("event_action")]
    public string Event_Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payload.
    /// </summary>
    [JsonProperty("payload")]
    public DtoPagerDutyPayload Payload { get; set; } = new();
}
