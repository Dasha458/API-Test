using System.Text.Json.Serialization;

namespace ApiTestingFramework.Business.Models;

public sealed class Geo
{
    [JsonPropertyName("lat")]
    public string? Lat { get; set; }

    [JsonPropertyName("lng")]
    public string? Lng { get; set; }
}
