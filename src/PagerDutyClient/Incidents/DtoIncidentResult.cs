//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoIncidentResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to transfer the incident result object from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <inheritdoc cref="DtoBaseResult"/>
/// <summary>
/// A class to transfer the incident result object from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
[DebuggerDisplay("{IncidentNumber}: {Status}")]
public class DtoIncidentResult : DtoBaseResult
{
    /// <summary>
    /// Gets or sets the incident number. The number of the incident. This is unique across your account.
    /// </summary>
    [JsonProperty("incident_number")]
    public int IncidentNumber { get; set; }

    /// <summary>
    /// Gets or sets the created at timestamp. The date/time the incident was first triggered.
    /// </summary>
    [JsonProperty("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    /// <summary>
    /// Gets the created at timestamp as <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonIgnore]
    public DateTimeOffset? TimestampCreatedAt => DateTimeOffset.TryParse(this.CreatedAt, out var timestamp) ? timestamp : null;

    /// <summary>
    /// Gets or sets the status. The current status of the incident. Allowed values: triggered, acknowledged, resolved.
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the title. A succinct description of the nature, symptoms, cause, or effect of the incident.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of pending_actions on the incident. A pending_action object contains a type of action which can be escalate, unacknowledge, resolve or urgency_change.
    /// A pending_action object contains at, the time at which the action will take place. An urgency_change pending_action will contain to, the urgency that the incident will change to.
    /// </summary>
    [JsonProperty("pending_actions")]
    public List<DtoPendingActionResult> PendingActions { get; set; } = new();

    /// <summary>
    /// Gets or sets the incident's de-duplication key.
    /// </summary>
    [JsonProperty("incident_key")]
    public string IncidentKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the service.
    /// </summary>
    [JsonProperty("service")]
    public DtoBaseResult? Service { get; set; }

    /// <summary>
    /// Gets or sets the assignments.
    /// </summary>
    [JsonProperty("assignments")]
    public List<DtoAssignmentResult> Assignments { get; set; } = new();

    /// <summary>
    /// Gets or sets the assigned via value. How the current incident assignments were decided. Note that direct_assignment incidents will not escalate up the attached escalation_policy.
    /// Allowed values: escalation_policy, direct_assignment.
    /// </summary>
    [JsonProperty("assigned_via")]
    public string AssignedVia { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the acknowledgments.
    /// </summary>
    [JsonProperty("acknowledgements")]
    public List<DtoAcknowledgmentResult> Acknowledgments { get; set; } = new();

    /// <summary>
    /// Gets or sets the last status change at timestamp. The time at which the status of the incident last changed.
    /// </summary>
    [JsonProperty("last_status_change_at")]
    public string LastStatusChangeAt { get; set; } = string.Empty;

    /// <summary>
    /// Gets the last status change at timestamp as <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonIgnore]
    public DateTimeOffset? TimestampLastStatusChangeAt => DateTimeOffset.TryParse(this.LastStatusChangeAt, out var timestamp) ? timestamp : null;

    /// <summary>
    /// Gets or sets the last status change by user. The agent (user, service or integration) that created or modified the Incident Log Entry.
    /// </summary>
    [JsonProperty("last_status_change_by")]
    public DtoBaseResult? LastStatusChangeBy { get; set; }

    /// <summary>
    /// Gets or sets the first trigger log entry.
    /// </summary>
    [JsonProperty("first_trigger_log_entry")]
    public DtoBaseResult? FirstTriggerLogEntry { get; set; }

    /// <summary>
    /// Gets or sets the escalation policy.
    /// </summary>
    [JsonProperty("escalation_policy")]
    public DtoBaseResult? EscalationPolicy { get; set; }

    /// <summary>
    /// Gets or sets the teams involved in the incident's lifecycle.
    /// </summary>
    [JsonProperty("teams")]
    public List<DtoBaseResult> Teams { get; set; } = new();

    /// <summary>
    /// Gets or sets the priority.
    /// </summary>
    [JsonProperty("priority")]
    public DtoBaseResult? Priority { get; set; }

    /// <summary>
    /// Gets or sets the current urgency of the incident. Allowed values: high, low.
    /// </summary>
    [JsonProperty("urgency")]
    public string Urgency { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resolve reason.
    /// </summary>
    [JsonProperty("resolve_reason")]
    public DtoResolveReasonResult? ResolveReason { get; set; }

    /// <summary>
    /// Gets or sets the alert count values.
    /// </summary>
    [JsonProperty("alert_counts")]
    public DtoAlertCountResult? AlertCount { get; set; }

    /// <summary>
    /// Gets or sets the conference bridge.
    /// </summary>
    [JsonProperty("conference_bridge")]
    public DtoConferenceBridgeResult? ConferenceBridge { get; set; }

    /// <summary>
    /// Gets or sets the body.
    /// </summary>
    [JsonProperty("body")]
    public DtoBodyResult? Body { get; set; }

    /// <summary>
    /// Gets or sets the occurrence.
    /// </summary>
    [JsonProperty("occurrence")]
    public DtoOccurenceResult? Occurence { get; set; }

    /// <summary>
    /// Gets or sets the incidents responders.
    /// </summary>
    [JsonProperty("incidents_responders")]
    public List<DtoIncidentsResponderResult> IncidentsResponders { get; set; } = new();

    /// <summary>
    /// Gets or sets the responder requests.
    /// </summary>
    [JsonProperty("responder_requests")]
    public List<DtoResponderRequestsResult> ResponderRequests { get; set; } = new();
}
