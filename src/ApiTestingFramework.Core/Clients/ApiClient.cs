using ApiTestingFramework.Core.Configuration;
using ApiTestingFramework.Core.Logging;
using RestSharp;

namespace ApiTestingFramework.Core.Clients;

public sealed class ApiClient : IApiClient, IDisposable
{
    private readonly RestClient _client;
    private readonly ITestLogger _logger;

    public ApiClient(IFrameworkConfig config, ITestLogger logger)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(logger);

        var options = new RestClientOptions(config.BaseUrl)
        {
            Timeout = TimeSpan.FromSeconds(config.RequestTimeoutSeconds),
            ThrowOnAnyError = false,
        };

        _client = new RestClient(options);
        _logger = logger;
    }

    public async Task<RestResponse> ExecuteAsync(
        RestRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        LogRequest(request);
        var response = await _client.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
        LogResponse(response);
        return response;
    }

    public async Task<RestResponse<T>> ExecuteAsync<T>(
        RestRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        LogRequest(request);
        var response = await _client.ExecuteAsync<T>(request, cancellationToken).ConfigureAwait(false);
        LogResponse(response);
        return response;
    }

    public void Dispose() => _client.Dispose();

    private void LogRequest(RestRequest request)
    {
        var url = _client.BuildUri(request);
        _logger.Info($"--> {request.Method} {url}");
    }

    private void LogResponse(RestResponse response)
    {
        _logger.Info($"<-- {(int)response.StatusCode} {response.StatusCode} ({response.ContentLength ?? 0} bytes)");

        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            _logger.Error($"RestSharp error: {response.ErrorMessage}");
        }
    }
}
