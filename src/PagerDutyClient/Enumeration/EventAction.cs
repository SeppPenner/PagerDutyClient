// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventAction.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The PagerDuty event action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Enumeration;

/// <summary>
/// The PagerDuty event action.
/// </summary>
internal enum EventAction
{
    /// <summary>
    /// The trigger event action.
    /// </summary>
    Trigger,

    /// <summary>
    /// The acknowledge event action.
    /// </summary>
    Acknowledge,

    /// <summary>
    /// The resolve event action.
    /// </summary>
    Resolve
}