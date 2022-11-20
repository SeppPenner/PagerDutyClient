//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoOccurenceResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A base class to transfer the occurence result objects from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A base class to transfer the occurence result objects from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoOccurenceResult
{
    /// <summary>
    /// Gets or sets the identifier of the service referenced.
    /// </summary>
    [JsonProperty("count")]
    public int Count { get; set; }

    /// <summary>
    /// Gets or sets the frequency.
    /// </summary>
    [JsonProperty("frequency")]
    public decimal Frequency { get; set; }

    /// <summary>
    /// Gets or sets the category. The classifcation of the Outlier Incident. The values can be one of ["novel", "rare", "frequent", "other"].
    /// "novel": It means this Incident hasn't occured. "rare": It means this Incident occurs with a low frequency.
    /// "frequent": It means this Incident occurs with a high frequency.
    /// "other": It means this Incident occurs with a medium frequency.
    /// Allowed values: novel, rare, frequent, other.
    /// </summary>
    [JsonProperty("category")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the since timestamp.
    /// </summary>
    [JsonProperty("since")]
    public string Since { get; set; } = string.Empty;

    /// <summary>
    /// Gets the since timestamp as <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonIgnore]
    public DateTimeOffset? SinceTimestamp => DateTimeOffset.TryParse(this.Since, out var timestamp) ? timestamp : null;

    /// <summary>
    /// Gets or sets the until timestamp.
    /// </summary>
    [JsonProperty("until")]
    public string Until { get; set; } = string.Empty;

    /// <summary>
    /// Gets the until timestamp as <see cref="DateTimeOffset"/>.
    /// </summary>
    [JsonIgnore]
    public DateTimeOffset? UntilTimestamp => DateTimeOffset.TryParse(this.Until, out var timestamp) ? timestamp : null;
}
