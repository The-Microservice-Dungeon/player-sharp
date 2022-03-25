using System.Net.Http.Headers;

namespace Sharp.Player.Util;

public class HttpLoggingHandler : DelegatingHandler
{
    private ILogger<HttpLoggingHandler> _logger;
    readonly string[] types = { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

    public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var id = $"req-{Guid.NewGuid().ToString().Substring(0, 8)}";
        string requestLine = $"{request.Method} - {request.RequestUri?.PathAndQuery} - {request.RequestUri?.Scheme}/{request.Version}";
        
        _logger.LogDebug("[{id}] (req): {RequestLine}", id, requestLine);
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("[{id}] (res): {Status}", id, response.StatusCode);

        return response;
    }

    bool IsTextBasedContentType(HttpHeaders headers)
    {
        IEnumerable<string> values;
        if (!headers.TryGetValues("Content-Type", out values))
            return false;
        var header = string.Join(" ", values).ToLowerInvariant();

        return types.Any(t => header.Contains(t));
    }

    private string FormatHeaders(HttpHeaders headers) => string.Join("\n",
        headers.Select(header => $"{header.Key}: {string.Join(",", header.Value)}"));

    private async Task<string> FormatContent(HttpContent content) => $"Content: ${await content.ReadAsStringAsync()}";
}