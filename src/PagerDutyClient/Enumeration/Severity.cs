// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Severity.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The PagerDuty severity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Enumeration;

/// <summary>
/// The PagerDuty severity.
/// </summary>
public enum Severity
{
    /// <summary>
    /// The information severity.
    /// </summary>
    Information,

    /// <summary>
    /// The warning severity.
    /// </summary>
    Warning,

    /// <summary>
    /// The error severity.
    /// </summary>
    Error,

    /// <summary>
    /// The critical severity.
    /// </summary>
    Critical
}