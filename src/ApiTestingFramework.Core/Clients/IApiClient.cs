using RestSharp;

namespace ApiTestingFramework.Core.Clients;

public interface IApiClient
{
    Task<RestResponse> ExecuteAsync(RestRequest request, CancellationToken cancellationToken = default);

    Task<RestResponse<T>> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default);
}
