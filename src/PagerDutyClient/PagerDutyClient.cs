// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagerDutyClient.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The PagerDuty client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Serilog;

    /// <summary>
    /// The PagerDuty client.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class PagerDutyClient
    {
        /// <summary>
        /// The Http client.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        ///     The JSON serializer settings.
        /// </summary>
        private readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagerDutyClient"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="baseAddress">The PagerDuty API base address.</param>
        public PagerDutyClient(ILogger logger, string baseAddress = "https://events.pagerduty.com")
        {
            this.logger = logger;
            this.httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };

            // Set JSON serializer settings
            this.serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.None
            };
        }

        /// <summary>
        /// Sends an alert to PagerDuty. Check <para>https://developer.pagerduty.com/docs/events-api-v2/trigger-events/</para> as well.
        /// </summary>
        /// <param name="eventAction">The event action.</param>
        /// <param name="routingKey">The routing key.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="source">The source.</param>
        /// <param name="summary">The summary.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        // ReSharper disable once UnusedMember.Global
        public async Task Send(EventAction eventAction, string routingKey, Severity severity, string source, string summary)
        {
            var dtoPagerDuty = new DtoPagerDuty
            {
                Event_Action = GetEventAction(eventAction),
                Routing_Key = routingKey,
                Payload = new DtoPagerDutyPayload { Severity = GetSeverity(severity), Source = source, Summary = summary }
            };

            await this.Send(dtoPagerDuty);
        }

        /// <summary>
        /// Sends an alert to PagerDuty. Check <para>https://developer.pagerduty.com/docs/events-api-v2/trigger-events/</para> as well.
        /// </summary>
        /// <param name="dtoPagerDuty">The DTO object to send.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        private async Task Send(DtoPagerDuty dtoPagerDuty)
        {
            try
            {
                var stringPayload = JsonConvert.SerializeObject(dtoPagerDuty, this.serializerSettings);
                var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var result = await this.httpClient.PostAsync("v2/enqueue", content).ConfigureAwait(false);

                if (result.IsSuccessStatusCode)
                {
                    this.logger.Information("Added PagerDuty alert with data {@Data}", dtoPagerDuty);
                }
                else
                {
                    this.logger.Error("Adding PagerDuty alert failed with data {@Data}", dtoPagerDuty);
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("An error occurred: {Exception}.", ex);
            }
        }

        /// <summary>
        /// Converts the <see cref="Severity"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="severity">The <see cref="Severity"/>.</param>
        /// <returns>A <see cref="Severity"/> as <see cref="string"/>.</returns>
        private static string GetSeverity(Severity severity)
        {
            return severity switch
            {
                Severity.Information => "info",
                Severity.Warning => "warning",
                Severity.Error => "error",
                Severity.Critical => "critical",
                _ => "info"
            };
        }

        /// <summary>
        /// Converts the <see cref="EventAction"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="eventAction">The <see cref="EventAction"/>.</param>
        /// <returns>A <see cref="EventAction"/> as <see cref="string"/>.</returns>
        private static string GetEventAction(EventAction eventAction)
        {
            return eventAction switch
            {
                EventAction.Trigger => "trigger",
                EventAction.Acknowledge => "acknowledge",
                EventAction.Resolve => "resolve",
                _ => "trigger"
            };
        }
    }
}