//--------------------------------------------------------------------------------------------------------------------
// <copyright file="DtoBaseResult.cs" company="Hämmer Electronics">
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
public class DtoBaseResult
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    [JsonProperty("id")]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the summary. A short-form, server-generated string that provides succinct,
    /// important information about an object suitable for primary labeling of an entity in a client.
    /// In many cases, this will be identical to name, though it is not intended to be an identifier.
    /// </summary>
    [JsonProperty("summary")]
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type. A string that determines the schema of the object.
    /// This must be the standard name for the entity, suffixed by _reference if the object is a reference.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the self url. The API show URL at which the object is accessible.
    /// </summary>
    [JsonProperty("self")]
    public string Self { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the HTML url. A URL at which the entity is uniquely displayed in the Web app.
    /// </summary>
    [JsonProperty("html_url")]
    public string HtmlUrl { get; set; } = string.Empty;
}
