// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataProvider.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class that provides the test data both test classes use.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Tests;

/// <summary>
/// A class that provides the test data both test classes use.
/// </summary>
internal static class TestDataProvider
{
    /// <summary>
    /// The API token of the REST API tests.
    /// </summary>
    public const string ApiToken = "y_NbAkKc66ryYTWUXYEu";

    /// <summary>
    /// The base address of the REST API.
    /// </summary>
    public const string ApiBaseAddress = "https://api.pagerduty.com";

    /// <summary>
    /// The base address of the events API.
    /// </summary>
    public const string EventsApiBaseAddress = "https://events.pagerduty.com";

    /// <summary>
    /// The routing key of the events API tests.
    /// </summary>
    public const string RoutingKey = "78e9279b9fa84b7b8e9f1ec7b9b85fef";

    /// <summary>
    /// The email address that identifies the caller of the REST API.
    /// </summary>
    public const string FromEmail = "someone@example.com";

    /// <summary>
    /// The incident identifier of the REST API tests.
    /// </summary>
    public const string IncidentIdentifier = "PT4KHLK";

    /// <summary>
    /// The source of the events API tests.
    /// </summary>
    public const string Source = "MyService";

    /// <summary>
    /// The summary of the events API tests.
    /// </summary>
    public const string Summary = "Some error.";

    /// <summary>
    /// The deduplication key of the events API tests.
    /// </summary>
    public const string DeduplicationKey = "SomeDeduplicationKey";

    /// <summary>
    /// The answer of the incidents endpoint, cut down to the properties the tests read.
    /// </summary>
    public const string IncidentsResponse = """
        {
            "incidents": [
                {
                    "id": "PT4KHLK",
                    "incident_number": 1234,
                    "status": "triggered",
                    "title": "Some error.",
                    "created_at": "2026-08-15T10:00:00Z"
                }
            ],
            "limit": 25,
            "offset": 0,
            "more": false,
            "total": 1
        }
        """;

    /// <summary>
    /// Gets a logger that writes nowhere. The clients need one, the tests never read it.
    /// </summary>
    /// <returns>A new <see cref="ILogger"/> without any sink.</returns>
    public static ILogger GetLogger()
    {
        return new LoggerConfiguration().CreateLogger();
    }
}
