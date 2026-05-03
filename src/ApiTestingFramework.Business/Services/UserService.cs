using ApiTestingFramework.Business.Endpoints;
using ApiTestingFramework.Business.Models;
using ApiTestingFramework.Core.Builders;
using ApiTestingFramework.Core.Clients;
using RestSharp;

namespace ApiTestingFramework.Business.Services;

public sealed class UserService
{
    private readonly IApiClient _client;

    public UserService(IApiClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    public Task<RestResponse> GetUsersRawAsync(CancellationToken cancellationToken = default)
    {
        var request = ApiRequestBuilder
            .For(ApiEndpoints.Users)
            .WithMethod(Method.Get)
            .Build();

        return _client.ExecuteAsync(request, cancellationToken);
    }

    public Task<RestResponse<List<User>>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var request = ApiRequestBuilder
            .For(ApiEndpoints.Users)
            .WithMethod(Method.Get)
            .Build();

        return _client.ExecuteAsync<List<User>>(request, cancellationToken);
    }

    public Task<RestResponse<User>> CreateUserAsync(
        CreateUserRequest payload,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(payload);

        var request = ApiRequestBuilder
            .For(ApiEndpoints.Users)
            .WithMethod(Method.Post)
            .WithJsonBody(payload)
            .Build();

        return _client.ExecuteAsync<User>(request, cancellationToken);
    }

    public Task<RestResponse> GetInvalidEndpointAsync(CancellationToken cancellationToken = default)
    {
        var request = ApiRequestBuilder
            .For(ApiEndpoints.InvalidEndpoint)
            .WithMethod(Method.Get)
            .Build();

        return _client.ExecuteAsync(request, cancellationToken);
    }
}
