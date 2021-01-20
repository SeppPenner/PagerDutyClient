// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Severity.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The PagerDuty severity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient
{
    /// <summary>
    /// The PagerDuty severity.
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// The PagerDuty information severity.
        /// </summary>
        Information,

        /// <summary>
        /// The PagerDuty warning severity.
        /// </summary>
        Warning,

        /// <summary>
        /// The PagerDuty error severity.
        /// </summary>
        Error,

        /// <summary>
        /// The PagerDuty critical severity.
        /// </summary>
        Critical
    }
}