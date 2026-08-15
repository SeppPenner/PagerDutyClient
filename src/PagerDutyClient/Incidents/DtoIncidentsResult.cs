//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoIncidentsResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to transfer the incidents result from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A class to transfer the incidents result from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoIncidentsResult
{
    /// <summary>
    /// Gets or sets the offset pagination property.
    /// </summary>
    [JsonProperty("offset")]
    public int Offset { get; set; }

    /// <summary>
    /// Gets or sets the limit pagination property.
    /// </summary>
    [JsonProperty("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether there are additional records to return or not.
    /// </summary>
    [JsonProperty("more")]
    public bool More { get; set; }

    /// <summary>
    /// Gets or sets the total number of records matching the given query.
    /// </summary>
    [JsonProperty("total")]
    public int? Total { get; set; }

    /// <summary>
    /// Gets or sets the incidents.
    /// </summary>
    [JsonProperty("incidents")]
    public List<DtoIncidentResult> Incidents { get; set; } = new();
}