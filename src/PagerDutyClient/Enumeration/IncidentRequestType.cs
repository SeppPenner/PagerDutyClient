// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncidentRequestType.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The PagerDuty incident request type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Enumeration;

/// <summary>
/// The PagerDuty incident request type.
/// </summary>
internal enum IncidentRequestType
{
    /// <summary>
    /// The acknowledged incident request type.
    /// </summary>
    Acknowledged,

    /// <summary>
    /// The resolved incident request type.
    /// </summary>
    Resolved
}