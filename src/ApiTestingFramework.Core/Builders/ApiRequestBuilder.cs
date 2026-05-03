using RestSharp;

namespace ApiTestingFramework.Core.Builders;

public sealed class ApiRequestBuilder
{
    private readonly RestRequest _request;

    private ApiRequestBuilder(string resource)
    {
        _request = new RestRequest(resource);
    }

    public static ApiRequestBuilder For(string resource)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resource);
        return new ApiRequestBuilder(resource);
    }

    public ApiRequestBuilder WithMethod(Method method)
    {
        _request.Method = method;
        return this;
    }

    public ApiRequestBuilder WithHeader(string name, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _request.AddHeader(name, value);
        return this;
    }

    public ApiRequestBuilder WithHeaders(IReadOnlyDictionary<string, string> headers)
    {
        ArgumentNullException.ThrowIfNull(headers);

        foreach (var (key, value) in headers)
        {
            _request.AddHeader(key, value);
        }

        return this;
    }

    public ApiRequestBuilder WithQueryParameter(string name, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _request.AddQueryParameter(name, value);
        return this;
    }

    public ApiRequestBuilder WithUrlSegment(string name, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _request.AddUrlSegment(name, value);
        return this;
    }

    public ApiRequestBuilder WithJsonBody<T>(T body) where T : class
    {
        ArgumentNullException.ThrowIfNull(body);
        _request.AddJsonBody(body);
        return this;
    }

    public ApiRequestBuilder WithTimeoutSeconds(int seconds)
    {
        if (seconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(seconds), "Timeout must be positive.");
        }

        _request.Timeout = TimeSpan.FromSeconds(seconds);
        return this;
    }

    public RestRequest Build() => _request;
}
