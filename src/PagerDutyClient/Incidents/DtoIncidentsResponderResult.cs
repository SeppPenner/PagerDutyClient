//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoIncidentsResponderResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A base class to transfer the incidents responder result objects from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A base class to transfer the incidents responder result objects from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoIncidentsResponderResult
{
    /// <summary>
    /// Gets or sets the status of the responder being added to the incident.
    /// </summary>
    [JsonProperty("state")]
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    [JsonProperty("user")]
    public DtoBaseResult? User { get; set; }

    /// <summary>
    /// Gets or sets the incident.
    /// </summary>
    [JsonProperty("incident")]
    public DtoBaseResult? Incident { get; set; }

    /// <summary>
    /// Gets or sets the updated at timestamp.
    /// </summary>
    [JsonProperty("updated_at")]
    public string UpdatedAt { get; set; } = string.Empty;

    /// <summary>
    /// Gets the updated at timestamp as <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonIgnore]
    public DateTimeOffset? UpdatedAtTimestamp => DateTimeOffset.TryParse(this.UpdatedAt, out var timestamp) ? timestamp : null;

    /// <summary>
    /// Gets or sets the message sent with the responder request.
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

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
}
