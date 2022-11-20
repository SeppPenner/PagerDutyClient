// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoEvent.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The DTO class to transfer the data for the PagerDuty events API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Events;

/// <summary>
/// The DTO class to transfer the data for the PagerDuty events API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
internal class DtoEvent
{
    /// <summary>
    /// Gets or sets the routing key.
    /// </summary>
    [JsonProperty("routing_key")]
    public string RoutingKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event action.
    /// </summary>
    [JsonProperty("event_action")]
    public string EventAction { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payload.
    /// </summary>
    [JsonProperty("payload")]
    public DtoEventPayload Payload { get; set; } = new();

    /// <summary>
    /// Gets or sets the deduplication key.
    /// </summary>
    [JsonProperty("dedup_key")]
    public string DeduplicationKey { get; set; } = string.Empty;
}