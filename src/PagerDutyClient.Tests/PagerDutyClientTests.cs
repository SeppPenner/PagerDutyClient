// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagerDutyClientTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the PagerDutyClient, the client of the REST API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Tests;

/// <summary>
/// A class to test the <see cref="RestClient"/>, the client of the REST API.
/// </summary>
[TestClass]
public sealed class PagerDutyClientTests
{
    /// <summary>
    /// Tests that the API token reaches the API with the token prefix the API expects.
    /// Without that prefix every call answers 401.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task GetIncidentsSendsTheApiTokenWithTheTokenPrefix()
    {
        var handler = new TestHttpMessageHandler(HttpStatusCode.OK, TestDataProvider.IncidentsResponse);
        var client = GetClient(handler);

        await client.GetIncidents();

        Assert.AreEqual("Token", handler.Request?.Headers.Authorization?.Scheme);
        Assert.AreEqual($"token={TestDataProvider.ApiToken}", handler.Request?.Headers.Authorization?.Parameter);
    }

    /// <summary>
    /// Tests that the sort order and the limit reach the API as query parameters.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task GetIncidentsUsesTheGivenSortByAndLimit()
    {
        var handler = new TestHttpMessageHandler(HttpStatusCode.OK, TestDataProvider.IncidentsResponse);
        var client = GetClient(handler);

        await client.GetIncidents("created_at:ASC", 50);

        Assert.AreEqual(HttpMethod.Get, handler.Request?.Method);
        Assert.AreEqual($"{TestDataProvider.ApiBaseAddress}/incidents?sort_by=created_at:ASC&limit=50", handler.RequestUrl);
    }

    /// <summary>
    /// Tests that a single service identifier reaches the API as one service_ids[] parameter.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task GetIncidentsForServiceSendsOneServiceIdParameter()
    {
        var handler = new TestHttpMessageHandler(HttpStatusCode.OK, TestDataProvider.IncidentsResponse);
        var client = GetClient(handler);

        await client.GetIncidentsForService("PIJ90N7");

        StringAssert.Contains(handler.RequestUrl, "service_ids[]=PIJ90N7");
    }

    /// <summary>
    /// Tests that several service identifiers reach the API as one service_ids[] parameter each.
    /// A comma separated list is read by the API as a single identifier.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task GetIncidentsForServicesRepeatsTheServiceIdParameter()
    {
        var handler = new TestHttpMessageHandler(HttpStatusCode.OK, TestDataProvider.IncidentsResponse);
        var client = GetClient(handler);

        await client.GetIncidentsForServices(["PIJ90N7", "PABC123"]);

        StringAssert.Contains(handler.RequestUrl, "service_ids[]=PIJ90N7&service_ids[]=PABC123");
        Assert.IsFalse(handler.RequestUrl.Contains("PIJ90N7,PABC123", StringComparison.Ordinal), "The identifiers must not be sent as a comma separated list.");
    }

    /// <summary>
    /// Tests that the answer of the API is deserialized into the result type.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task GetIncidentsDeserializesTheAnswer()
    {
        var handler = new TestHttpMessageHandler(HttpStatusCode.OK, TestDataProvider.IncidentsResponse);
        var client = GetClient(handler);

        var result = await client.GetIncidents();

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Total);
        Assert.AreEqual(25, result.Limit);
        Assert.IsFalse(result.More);
        Assert.AreEqual(1, result.Incidents.Count);
        Assert.AreEqual(TestDataProvider.IncidentIdentifier, result.Incidents[0].Identifier);
        Assert.AreEqual(1234, result.Incidents[0].IncidentNumber);
        Assert.AreEqual("triggered", result.Incidents[0].Status);
        Assert.AreEqual("Some error.", result.Incidents[0].Title);
    }

    /// <summary>
    /// Tests that the timestamp of the answer is parsed into the read only property that offers it typed.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task GetIncidentsParsesTheCreatedAtTimestamp()
    {
        var handler = new TestHttpMessageHandler(HttpStatusCode.OK, TestDataProvider.IncidentsResponse);
        var client = GetClient(handler);

        var result = await client.GetIncidents();

        var timestamp = result?.Incidents[0].TimestampCreatedAt;
        Assert.IsNotNull(timestamp);
        Assert.AreEqual(new DateTimeOffset(2026, 8, 15, 10, 0, 0, TimeSpan.Zero), timestamp.Value.ToUniversalTime());
    }

    /// <summary>
    /// Tests that an error status code of the API reaches the caller as null and not as an exception.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task GetIncidentsReturnsNullOnAnErrorStatusCode()
    {
        var handler = new TestHttpMessageHandler(HttpStatusCode.Unauthorized, """{"error":{"message":"Caller is not authorized"}}""");
        var client = GetClient(handler);

        var result = await client.GetIncidents();

        Assert.IsNull(result);
    }

    /// <summary>
    /// Tests that an acknowledgment is a PUT of the acknowledged status to the incident.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task AcknowledgeAlertPutsTheAcknowledgedStatus()
    {
        var handler = new TestHttpMessageHandler();
        var client = GetClient(handler);

        await client.AcknowledgeAlert(TestDataProvider.IncidentIdentifier, TestDataProvider.FromEmail);

        Assert.AreEqual(HttpMethod.Put, handler.Request?.Method);
        Assert.AreEqual($"{TestDataProvider.ApiBaseAddress}/incidents/{TestDataProvider.IncidentIdentifier}", handler.RequestUrl);
        Assert.AreEqual(TestDataProvider.FromEmail, handler.Request?.Headers.From);

        var payload = JObject.Parse(handler.RequestContent ?? string.Empty);
        Assert.AreEqual("incident_reference", (string?)payload["incident"]?["type"]);
        Assert.AreEqual("acknowledged", (string?)payload["incident"]?["status"]);
        Assert.IsNull(payload["incident"]?["resolution"], "An acknowledgment has no resolution.");
    }

    /// <summary>
    /// Tests that a resolve is a PUT of the resolved status and the resolution text.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task ResolveAlertPutsTheResolvedStatusAndTheResolution()
    {
        var handler = new TestHttpMessageHandler();
        var client = GetClient(handler);

        await client.ResolveAlert(TestDataProvider.IncidentIdentifier, TestDataProvider.FromEmail, "Fixed by hand.");

        var payload = JObject.Parse(handler.RequestContent ?? string.Empty);
        Assert.AreEqual("resolved", (string?)payload["incident"]?["status"]);
        Assert.AreEqual("Fixed by hand.", (string?)payload["incident"]?["resolution"]);
    }

    /// <summary>
    /// Tests that an empty list of service identifiers is rejected before a request is sent.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task GetIncidentsForServicesThrowsOnAnEmptyList()
    {
        var handler = new TestHttpMessageHandler();
        var client = GetClient(handler);

        await Assert.ThrowsExactlyAsync<ArgumentException>(() => client.GetIncidentsForServices([]));

        Assert.IsNull(handler.Request, "No request may be sent for invalid arguments.");
    }

    /// <summary>
    /// Tests that an empty sort order is rejected.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task GetIncidentsThrowsOnAnEmptySortBy()
    {
        var client = GetClient(new TestHttpMessageHandler());

        await Assert.ThrowsExactlyAsync<ArgumentException>(() => client.GetIncidents(" "));
    }

    /// <summary>
    /// Tests that a limit of zero or less is rejected.
    /// </summary>
    /// <param name="limit">The invalid limit.</param>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public async Task GetIncidentsThrowsOnAnInvalidLimit(int limit)
    {
        var client = GetClient(new TestHttpMessageHandler());

        await Assert.ThrowsExactlyAsync<ArgumentException>(() => client.GetIncidents("created_at:DESC", limit));
    }

    /// <summary>
    /// Tests that an empty incident identifier is rejected.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task AcknowledgeAlertThrowsOnAnEmptyIdentifier()
    {
        var client = GetClient(new TestHttpMessageHandler());

        await Assert.ThrowsExactlyAsync<ArgumentException>(() => client.AcknowledgeAlert(string.Empty, TestDataProvider.FromEmail));
    }

    /// <summary>
    /// Tests that an empty from email address is rejected.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task AcknowledgeAlertThrowsOnAnEmptyFromEmail()
    {
        var client = GetClient(new TestHttpMessageHandler());

        await Assert.ThrowsExactlyAsync<ArgumentException>(() => client.AcknowledgeAlert(TestDataProvider.IncidentIdentifier, string.Empty));
    }

    /// <summary>
    /// Tests that an empty resolution is rejected, because the API needs one to resolve an incident.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task ResolveAlertThrowsOnAnEmptyResolution()
    {
        var client = GetClient(new TestHttpMessageHandler());

        await Assert.ThrowsExactlyAsync<ArgumentException>(
            () => client.ResolveAlert(TestDataProvider.IncidentIdentifier, TestDataProvider.FromEmail, string.Empty));
    }

    /// <summary>
    /// Gets a new <see cref="RestClient"/> that answers from the given handler instead of the network.
    /// </summary>
    /// <param name="handler">The handler that answers the requests.</param>
    /// <returns>A new <see cref="RestClient"/>.</returns>
    private static RestClient GetClient(TestHttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri(TestDataProvider.ApiBaseAddress) };
        return new RestClient(TestDataProvider.GetLogger(), TestDataProvider.ApiToken, httpClient);
    }
}
