// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagerDutyClientV2.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The PagerDuty client for V2 API calls (Events API).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient;

/// <summary>
/// The PagerDuty client for V2 API calls (Events API).
/// </summary>
public class PagerDutyClientV2
{
    /// <summary>
    /// The events HTTP client.
    /// </summary>
    private readonly HttpClient eventsHttpClient;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger logger;

    /// <summary>
    /// The JSON serializer settings.
    /// </summary>
    private readonly JsonSerializerSettings serializerSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="PagerDutyClient"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="eventsApiBaseAddress">The PagerDuty events API base address.</param>
    public PagerDutyClientV2(ILogger logger, string eventsApiBaseAddress = "https://events.pagerduty.com")
    {
        this.logger = logger;
        this.eventsHttpClient = new HttpClient { BaseAddress = new Uri(eventsApiBaseAddress) };

        // Set JSON serializer settings
        this.serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    /// <summary>
    /// Resolves an alert. Check <para>https://developer.pagerduty.com/docs/events-api-v2/trigger-events/</para> as well.
    /// </summary>
    /// <param name="routingKey">The routing key.</param>
    /// <param name="severity">The severity.</param>
    /// <param name="source">The source.</param>
    /// <param name="summary">The summary.</param>
    /// <param name="deduplicationKey">The deduplication key.</param>
    public async Task ResolveAlert(string routingKey, Severity severity, string source, string summary, string deduplicationKey)
    {
        var dtoPagerDuty = new DtoEvent
        {
            EventAction = GetEventAction(EventAction.Resolve),
            RoutingKey = routingKey,
            Payload = new DtoEventPayload { Severity = GetSeverity(severity), Source = source, Summary = summary },
            DeduplicationKey = deduplicationKey
        };

        await this.SendToEventsApi(dtoPagerDuty);
    }

    /// <summary>
    /// Acknowledges an alert. Check <para>https://developer.pagerduty.com/docs/events-api-v2/trigger-events/</para> as well.
    /// </summary>
    /// <param name="routingKey">The routing key.</param>
    /// <param name="severity">The severity.</param>
    /// <param name="source">The source.</param>
    /// <param name="summary">The summary.</param>
    /// <param name="deduplicationKey">The deduplication key.</param>
    public async Task AcknowledgeAlert(string routingKey, Severity severity, string source, string summary, string deduplicationKey)
    {
        var dtoPagerDuty = new DtoEvent
        {
            EventAction = GetEventAction(EventAction.Acknowledge),
            RoutingKey = routingKey,
            Payload = new DtoEventPayload { Severity = GetSeverity(severity), Source = source, Summary = summary },
            DeduplicationKey = deduplicationKey
        };

        await this.SendToEventsApi(dtoPagerDuty);
    }

    /// <summary>
    /// Triggers an alert. Check <para>https://developer.pagerduty.com/docs/events-api-v2/trigger-events/</para> as well.
    /// </summary>
    /// <param name="routingKey">The routing key.</param>
    /// <param name="severity">The severity.</param>
    /// <param name="source">The source.</param>
    /// <param name="summary">The summary.</param>
    public async Task TriggerAlert(string routingKey, Severity severity, string source, string summary)
    {
        var dtoPagerDuty = new DtoEvent
        {
            EventAction = GetEventAction(EventAction.Trigger),
            RoutingKey = routingKey,
            Payload = new DtoEventPayload { Severity = GetSeverity(severity), Source = source, Summary = summary }
        };

        await this.SendToEventsApi(dtoPagerDuty);
    }

    /// <summary>
    /// Sends an alert to PagerDuty. Check <para>https://developer.pagerduty.com/docs/events-api-v2/trigger-events/</para> as well.
    /// </summary>
    /// <param name="dtoPagerDuty">The DTO object to send.</param>
    private async Task SendToEventsApi(DtoEvent dtoPagerDuty)
    {
        try
        {
            var stringPayload = JsonConvert.SerializeObject(dtoPagerDuty, this.serializerSettings);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var result = await this.eventsHttpClient.PostAsync("v2/enqueue", content).ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {
                this.logger.Information("Added PagerDuty alert with data {@Data}", dtoPagerDuty);
            }
            else
            {
                var resultString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.logger.Error("Adding PagerDuty alert failed with data {@Data}, Message {Message}", dtoPagerDuty, resultString);
            }
        }
        catch (Exception ex)
        {
            this.logger.Error("An error occurred: {Exception}.", ex);
        }
    }

    /// <summary>
    /// Converts the event <see cref="Severity"/> to a <see cref="string"/>.
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
