// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagerDutyClientV2Tests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="PagerDutyClientV2"/>, the client of the events API.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Tests;

/// <summary>
/// A class to test the <see cref="PagerDutyClientV2"/>, the client of the events API.
/// </summary>
[TestClass]
public sealed class PagerDutyClientV2Tests
{
    /// <summary>
    /// Tests that a trigger is a POST to the enqueue endpoint of the events API.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task TriggerAlertPostsToTheEnqueueEndpoint()
    {
        var handler = new TestHttpMessageHandler();
        var client = GetClient(handler);

        await client.TriggerAlert(TestDataProvider.RoutingKey, Severity.Critical, TestDataProvider.Source, TestDataProvider.Summary);

        Assert.AreEqual(HttpMethod.Post, handler.Request?.Method);
        Assert.AreEqual($"{TestDataProvider.EventsApiBaseAddress}/v2/enqueue", handler.RequestUrl);
    }

    /// <summary>
    /// Tests that a trigger sends the routing key, the action and the whole payload.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task TriggerAlertSendsTheRoutingKeyAndThePayload()
    {
        var handler = new TestHttpMessageHandler();
        var client = GetClient(handler);

        await client.TriggerAlert(TestDataProvider.RoutingKey, Severity.Error, TestDataProvider.Source, TestDataProvider.Summary);

        var payload = JObject.Parse(handler.RequestContent ?? string.Empty);
        Assert.AreEqual(TestDataProvider.RoutingKey, (string?)payload["routing_key"]);
        Assert.AreEqual("trigger", (string?)payload["event_action"]);
        Assert.AreEqual("error", (string?)payload["payload"]?["severity"]);
        Assert.AreEqual(TestDataProvider.Source, (string?)payload["payload"]?["source"]);
        Assert.AreEqual(TestDataProvider.Summary, (string?)payload["payload"]?["summary"]);
    }

    /// <summary>
    /// Tests that a trigger leaves the deduplication key out of the payload instead of sending an empty one,
    /// so that the API generates a key of its own.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task TriggerAlertLeavesTheDeduplicationKeyOutOfThePayload()
    {
        var handler = new TestHttpMessageHandler();
        var client = GetClient(handler);

        await client.TriggerAlert(TestDataProvider.RoutingKey, Severity.Critical, TestDataProvider.Source, TestDataProvider.Summary);

        var payload = JObject.Parse(handler.RequestContent ?? string.Empty);
        Assert.IsNull(payload["dedup_key"], "A trigger must not send a deduplication key at all, not even an empty one.");
    }

    /// <summary>
    /// Tests that an acknowledgment sends the action and the deduplication key it was given.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task AcknowledgeAlertSendsTheAcknowledgeActionAndTheDeduplicationKey()
    {
        var handler = new TestHttpMessageHandler();
        var client = GetClient(handler);

        await client.AcknowledgeAlert(
            TestDataProvider.RoutingKey,
            Severity.Warning,
            TestDataProvider.Source,
            TestDataProvider.Summary,
            TestDataProvider.DeduplicationKey);

        var payload = JObject.Parse(handler.RequestContent ?? string.Empty);
        Assert.AreEqual("acknowledge", (string?)payload["event_action"]);
        Assert.AreEqual(TestDataProvider.DeduplicationKey, (string?)payload["dedup_key"]);
    }

    /// <summary>
    /// Tests that a resolve sends the action and the deduplication key it was given.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task ResolveAlertSendsTheResolveActionAndTheDeduplicationKey()
    {
        var handler = new TestHttpMessageHandler();
        var client = GetClient(handler);

        await client.ResolveAlert(
            TestDataProvider.RoutingKey,
            Severity.Information,
            TestDataProvider.Source,
            TestDataProvider.Summary,
            TestDataProvider.DeduplicationKey);

        var payload = JObject.Parse(handler.RequestContent ?? string.Empty);
        Assert.AreEqual("resolve", (string?)payload["event_action"]);
        Assert.AreEqual(TestDataProvider.DeduplicationKey, (string?)payload["dedup_key"]);
    }

    /// <summary>
    /// Tests that every severity reaches the API as the value the API knows.
    /// </summary>
    /// <param name="severity">The severity to send.</param>
    /// <param name="expectedValue">The value the API is supposed to receive.</param>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    [DataRow(Severity.Information, "info")]
    [DataRow(Severity.Warning, "warning")]
    [DataRow(Severity.Error, "error")]
    [DataRow(Severity.Critical, "critical")]
    public async Task TriggerAlertMapsEverySeverity(Severity severity, string expectedValue)
    {
        var handler = new TestHttpMessageHandler();
        var client = GetClient(handler);

        await client.TriggerAlert(TestDataProvider.RoutingKey, severity, TestDataProvider.Source, TestDataProvider.Summary);

        var payload = JObject.Parse(handler.RequestContent ?? string.Empty);
        Assert.AreEqual(expectedValue, (string?)payload["payload"]?["severity"]);
    }

    /// <summary>
    /// Tests that an error status code of the API does not reach the caller as an exception.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
    [TestMethod]
    public async Task AnErrorStatusCodeDoesNotThrow()
    {
        var handler = new TestHttpMessageHandler(HttpStatusCode.BadRequest, """{"errors":["Event object is invalid"]}""");
        var client = GetClient(handler);

        await client.TriggerAlert(TestDataProvider.RoutingKey, Severity.Critical, TestDataProvider.Source, TestDataProvider.Summary);

        Assert.IsNotNull(handler.Request, "The request was not sent at all.");
    }

    /// <summary>
    /// Gets a new <see cref="PagerDutyClientV2"/> that answers from the given handler instead of the network.
    /// </summary>
    /// <param name="handler">The handler that answers the requests.</param>
    /// <returns>A new <see cref="PagerDutyClientV2"/>.</returns>
    private static PagerDutyClientV2 GetClient(TestHttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri(TestDataProvider.EventsApiBaseAddress) };
        return new PagerDutyClientV2(TestDataProvider.GetLogger(), httpClient);
    }
}
