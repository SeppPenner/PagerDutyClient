// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestHttpMessageHandler.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   An HTTP message handler that answers without a network connection and records the request it was given.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PagerDutyClient.Tests;

/// <summary>
/// An HTTP message handler that answers without a network connection and records the request it was given.
/// </summary>
internal sealed class TestHttpMessageHandler : HttpMessageHandler
{
    /// <summary>
    /// The status code of the answer.
    /// </summary>
    private readonly HttpStatusCode statusCode;

    /// <summary>
    /// The body of the answer.
    /// </summary>
    private readonly string responseContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestHttpMessageHandler"/> class.
    /// </summary>
    /// <param name="statusCode">The status code of the answer, defaults to 200.</param>
    /// <param name="responseContent">The body of the answer, defaults to an empty JSON object.</param>
    public TestHttpMessageHandler(HttpStatusCode statusCode = HttpStatusCode.OK, string responseContent = "{}")
    {
        this.statusCode = statusCode;
        this.responseContent = responseContent;
    }

    /// <summary>
    /// Gets the request the handler was given, or null if it was never called.
    /// </summary>
    public HttpRequestMessage? Request { get; private set; }

    /// <summary>
    /// Gets the body of the request the handler was given, or null if the request had none.
    /// </summary>
    public string? RequestContent { get; private set; }

    /// <summary>
    /// Gets the url of the request with the percent encoding resolved, so that a test can read the query as it was built.
    /// </summary>
    public string RequestUrl => Uri.UnescapeDataString(this.Request?.RequestUri?.ToString() ?? string.Empty);

    /// <summary>
    /// Records the request and answers it from the values the constructor was given.
    /// </summary>
    /// <param name="request">The request to answer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The prepared <see cref="HttpResponseMessage"/>.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        this.Request = request;

        if (request.Content is not null)
        {
            this.RequestContent = await request.Content.ReadAsStringAsync(cancellationToken);
        }

        return new HttpResponseMessage(this.statusCode)
        {
            Content = new StringContent(this.responseContent, Encoding.UTF8, "application/json")
        };
    }
}
