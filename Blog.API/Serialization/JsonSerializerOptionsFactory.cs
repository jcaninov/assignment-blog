using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blog.API.Serialization;

public static class JsonSerializerOptionsFactory
{
    public static JsonSerializerOptions Create() => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}
