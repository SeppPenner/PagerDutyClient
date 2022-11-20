//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoResponderRequestsResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A base class to transfer the responder requests result objects from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A base class to transfer the responder requests result objects from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoResponderRequestsResult
{
    /// <summary>
    /// Gets or sets the incident.
    /// </summary>
    [JsonProperty("incident")]
    public DtoBaseResult? Incident { get; set; }

    /// <summary>
    /// Gets or sets the requester.
    /// </summary>
    [JsonProperty("requester")]
    public DtoBaseResult? Requester { get; set; }

    /// <summary>
    /// Gets or sets the requested at timestamp.
    /// </summary>
    [JsonProperty("requested_at")]
    public string RequestedAt { get; set; } = string.Empty;

    /// <summary>
    /// Gets the requested at timestamp as <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonIgnore]
    public DateTimeOffset? RequestedAtTimestamp => DateTimeOffset.TryParse(this.RequestedAt, out var timestamp) ? timestamp : null;

    /// <summary>
    /// Gets or sets the message sent with the responder request.
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the array of targets the responder request is being sent to.
    /// </summary>
    [JsonProperty("responder_request_targets")]
    public List<DtoResponderRequestTargetsResult> ResponderRequestTargets { get; set; } = new();
}
