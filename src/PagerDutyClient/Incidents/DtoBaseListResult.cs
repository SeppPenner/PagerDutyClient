//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoBaseListResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A base class to transfer the result objects from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A base class to transfer the result objects from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoBaseListResult
{
    /// <summary>
    /// Gets or sets the at timestamp.
    /// </summary>
    [JsonProperty("at")]
    public string At { get; set; } = string.Empty;

    /// <summary>
    /// Gets the at timestamp as <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonIgnore]
    public DateTimeOffset? AtTimestamp => DateTimeOffset.TryParse(this.At, out var timestamp) ? timestamp : null;
}
