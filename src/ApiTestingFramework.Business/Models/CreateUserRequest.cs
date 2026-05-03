using System.Text.Json.Serialization;

namespace ApiTestingFramework.Business.Models;

public sealed class CreateUserRequest
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("username")]
    public string Username { get; init; } = string.Empty;
}
