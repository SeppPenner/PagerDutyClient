// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventAction.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The PagerDuty event action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient
{
    /// <summary>
    /// The PagerDuty event action.
    /// </summary>
    public enum EventAction
    {
        /// <summary>
        /// The PagerDuty trigger event action.
        /// </summary>
        Trigger,

        /// <summary>
        /// The PagerDuty acknowledge event action.
        /// </summary>
        Acknowledge,

        /// <summary>
        /// The PagerDuty resolve event action.
        /// </summary>
        Resolve
    }
}