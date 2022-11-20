//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoAssignmentResult.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to transfer the assignment result object from the PagerDuty API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Incidents;

/// <summary>
/// A class to transfer the assignment result object from the PagerDuty API.
/// </summary>
[JsonSchemaFlatten]
[Serializable]
public class DtoAssignmentResult : DtoBaseListResult
{
    /// <summary>
    /// Gets or sets the assignee.
    /// </summary>
    [JsonProperty("assignee")]
    public DtoBaseResult? Assignee { get; set; }
}
