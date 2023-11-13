// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagerDutyClient.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The PagerDuty client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient;

/// <summary>
/// The PagerDuty client.
/// </summary>
public class PagerDutyClient
{
    /// <summary>
    /// The HTTP client.
    /// </summary>
    private readonly HttpClient httpClient;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger logger;

    /// <summary>
    /// The JSON serializer settings.
    /// </summary>
    private readonly JsonSerializerSettings serializerSettings;

    /// <summary>
    /// The API token.
    /// </summary>
    private readonly string apiToken;

    /// <summary>
    /// Initializes a new instance of the <see cref="PagerDutyClient"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="apiToken">The API token.</param>
    /// <param name="apiBaseAddress">The PagerDuty API base address.</param>
    public PagerDutyClient(ILogger logger, string apiToken, string apiBaseAddress = "https://api.pagerduty.com")
    {
        this.logger = logger;
        this.apiToken = apiToken;
        this.httpClient = new HttpClient { BaseAddress = new Uri(apiBaseAddress) };

        // Set JSON serializer settings
        this.serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    /// <summary>
    /// Gets the incidents for all services with the given identifiers. Check <para>https://developer.pagerduty.com/api-reference/</para> as well.
    /// </summary>
    /// <param name="serviceIds">The service identifiers.</param>
    /// <param name="sortBy">The sort by filter, defaults to 'created_at:DESC'.</param>
    /// <param name="limit">The result limit, defaults to 25.</param>
    /// <exception cref="ArgumentException">Thrown if any of the given arguments is invalid.</exception>
    /// <returns>The found <see cref="DtoIncidentsResult"/> or null.</returns>
    public async Task<DtoIncidentsResult?> GetIncidentsForServices(List<string> serviceIds, string sortBy = "created_at:DESC", int limit = 25)
    {
        if (!serviceIds.Any())
        {
            throw new ArgumentException("The service identifiers are empty.", nameof(serviceIds));
        }

        if (string.IsNullOrWhiteSpace(sortBy))
        {
            throw new ArgumentException("The sort by filter is empty.", nameof(sortBy));
        }

        if (limit <= 0)
        {
            throw new ArgumentException("The limit is invalid.", nameof(limit));
        }

        return await this.GetFromApi<DtoIncidentsResult?>($"incidents?sort_by={sortBy}&limit={limit}&service_ids[]={string.Join(",", serviceIds)}");
    }

    /// <summary>
    /// Gets the incidents for all services with the given identifier. Check <para>https://developer.pagerduty.com/api-reference/</para> as well.
    /// </summary>
    /// <param name="serviceId">The service identifier.</param>
    /// <param name="sortBy">The sort by filter, defaults to 'created_at:DESC'.</param>
    /// <param name="limit">The result limit, defaults to 25.</param>
    /// <exception cref="ArgumentException">Thrown if any of the given arguments is invalid.</exception>
    /// <returns>The found <see cref="DtoIncidentsResult"/> or null.</returns>
    public async Task<DtoIncidentsResult?> GetIncidentsForService(string serviceId, string sortBy = "created_at:DESC", int limit = 25)
    {
        if (string.IsNullOrWhiteSpace(serviceId))
        {
            throw new ArgumentException("The service identifier is empty.", nameof(serviceId));
        }

        if (string.IsNullOrWhiteSpace(sortBy))
        {
            throw new ArgumentException("The sort by filter is empty.", nameof(sortBy));
        }

        if (limit <= 0)
        {
            throw new ArgumentException("The limit is invalid.", nameof(limit));
        }

        return await this.GetFromApi<DtoIncidentsResult?>($"incidents?sort_by={sortBy}&limit={limit}&service_ids[]={serviceId}");
    }

    /// <summary>
    /// Gets the incidents for all services. Check <para>https://developer.pagerduty.com/api-reference/</para> as well.
    /// </summary>
    /// <param name="sortBy">The sort by filter, defaults to 'created_at:DESC'.</param>
    /// <param name="limit">The result limit, defaults to 25.</param>
    /// <exception cref="ArgumentException">Thrown if any of the given arguments is invalid.</exception>
    /// <returns>The found <see cref="DtoIncidentsResult"/> or null.</returns>
    public async Task<DtoIncidentsResult?> GetIncidents(string sortBy = "created_at:DESC", int limit = 25)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            throw new ArgumentException("The sort by filter is empty.", nameof(sortBy));
        }

        if (limit <= 0)
        {
            throw new ArgumentException("The limit is invalid.", nameof(limit));
        }

        return await this.GetFromApi<DtoIncidentsResult?>($"incidents?sort_by={sortBy}&limit={limit}");
    }

    /// <summary>
    /// Resolves the alert. Check <para>https://developer.pagerduty.com/api-reference/</para> as well.
    /// </summary>
    /// <param name="identifier">The alert identifier.</param>
    /// <param name="fromEmail">The from email address.</param>
    /// <param name="resolution">The resolution if any (Needed for resolve only).</param>
    /// <exception cref="ArgumentException">Thrown if any of the given arguments is invalid.</exception>
    /// <returns>The found <see cref="DtoIncidentResult"/> or null.</returns>
    public async Task<DtoIncidentResult?> ResolveAlert(string identifier, string fromEmail, string resolution)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            throw new ArgumentException("The identifier is empty.", nameof(identifier));
        }

        if (string.IsNullOrWhiteSpace(fromEmail))
        {
            throw new ArgumentException("The from email is empty.", nameof(fromEmail));
        }

        if (string.IsNullOrWhiteSpace(resolution))
        {
            throw new ArgumentException("The resolution is empty.", nameof(resolution));
        }

        return await this.PutToApi<DtoIncidentResult?>($"incidents/{identifier}", fromEmail, IncidentRequestType.Resolved, resolution);
    }

    /// <summary>
    /// Acknowledges the alert. Check <para>https://developer.pagerduty.com/api-reference/</para> as well.
    /// </summary>
    /// <param name="identifier">The alert identifier.</param>
    /// <param name="fromEmail">The from email address.</param>
    /// <exception cref="ArgumentException">Thrown if any of the given arguments is invalid.</exception>
    /// <returns>The found <see cref="DtoIncidentResult"/> or null.</returns>
    public async Task<DtoIncidentResult?> AcknowledgeAlert(string identifier, string fromEmail)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            throw new ArgumentException("The identifier is empty.", nameof(identifier));
        }

        if (string.IsNullOrWhiteSpace(fromEmail))
        {
            throw new ArgumentException("The from email is empty.", nameof(fromEmail));
        }

        return await this.PutToApi<DtoIncidentResult?>($"incidents/{identifier}", fromEmail, IncidentRequestType.Acknowledged);
    }

    /// <summary>
    /// Puts data to the PagerDuty API. Check <para>https://developer.pagerduty.com/api-reference/</para> as well.
    /// </summary>
    /// <typeparam name="T">The type of object.</typeparam>
    /// <param name="url">The url.</param>
    /// <param name="incidentRequestType">The incident request type.</param>
    /// <param name="fromEmail">The from email address.</param>
    /// <param name="resolution">The resolution if any (Needed for resolve only).</param>
    /// <returns>A new object of type <c>T</c> or null.</returns>
    private async Task<T?> PutToApi<T>(string url, string fromEmail, IncidentRequestType incidentRequestType, string? resolution = null)
    {
        try
        {
            if (this.httpClient.DefaultRequestHeaders.Authorization is null)
            {
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(HeaderKeys.Token, this.apiToken);
            }

            if (!this.httpClient.DefaultRequestHeaders.Contains(HeaderKeys.From))
            {
                this.httpClient.DefaultRequestHeaders.Add(HeaderKeys.From, fromEmail);
            }

            var dtoPagerDuty = new DtoIncidentRequest
            {
                Incident = new()
                {
                    Type = "incident_reference",
                    Status = GetIncidentRequestType(incidentRequestType),
                    Resolution = incidentRequestType == IncidentRequestType.Resolved ? resolution : null
                }
            };
            var stringPayload = JsonConvert.SerializeObject(dtoPagerDuty, this.serializerSettings);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var result = await this.httpClient.PutAsync(url, content).ConfigureAwait(false);
            var stringValue = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {
                this.logger.Information("Got PagerDuty data {@Data}", result);
                return JsonConvert.DeserializeObject<T>(stringValue);
            }
            else
            {
                this.logger.Error("Getting PagerDuty data failed {@Data}, Message {Message}", result, stringValue);
            }
        }
        catch (Exception ex)
        {
            this.logger.Error("An error occurred: {Exception}.", ex);
        }

        return default;
    }

    /// <summary>
    /// Gets data from the PagerDuty API. Check <para>https://developer.pagerduty.com/api-reference/</para> as well.
    /// </summary>
    /// <typeparam name="T">The type of object.</typeparam>
    /// <param name="url">The url.</param>
    /// <returns>A new object of type <c>T</c> or null.</returns>
    private async Task<T?> GetFromApi<T>(string url)
    {
        try
        {
            if (this.httpClient.DefaultRequestHeaders.Authorization is null)
            {
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(HeaderKeys.Token, this.apiToken);
            }

            var result = await this.httpClient.GetAsync(url).ConfigureAwait(false);
            var stringValue = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (result.IsSuccessStatusCode)
            {
                this.logger.Information("Got PagerDuty data {@Data}", result);
                return JsonConvert.DeserializeObject<T>(stringValue);
            }
            else
            {
                this.logger.Error("Getting PagerDuty data failed {@Data}, Message {Message}", result, stringValue);
            }
        }
        catch (Exception ex)
        {
            this.logger.Error("An error occurred: {Exception}.", ex);
        }

        return default;
    }

    /// <summary>
    /// Converts the <see cref="IncidentRequestType"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="incidentRequestType">The <see cref="IncidentRequestType"/>.</param>
    /// <returns>A <see cref="IncidentRequestType"/> as <see cref="string"/>.</returns>
    private static string GetIncidentRequestType(IncidentRequestType incidentRequestType)
    {
        return incidentRequestType switch
        {
            IncidentRequestType.Resolved => "resolved",
            IncidentRequestType.Acknowledged => "acknowledged",
            _ => "acknowledged"
        };
    }
}
