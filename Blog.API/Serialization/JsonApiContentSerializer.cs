using System.Text.Json;

namespace Blog.API.Serialization;

public sealed class JsonApiContentSerializer : IApiContentSerializer
{
    public const string MediaType = "application/json";

    private readonly JsonSerializerOptions _options;

    public JsonApiContentSerializer(JsonSerializerOptions options) => _options = options;

    public ApiContentFormat Format => ApiContentFormat.Json;

    public string ContentType => MediaType;

    public async ValueTask<T?> DeserializeAsync<T>(Stream body, CancellationToken cancellationToken = default)
    {
        if (body.CanSeek && body.Length == 0)
            return default;

        return await JsonSerializer.DeserializeAsync<T>(body, _options, cancellationToken);
    }

    public async ValueTask SerializeAsync<T>(Stream response, T value, CancellationToken cancellationToken = default)
    {
        await JsonSerializer.SerializeAsync(response, value, _options, cancellationToken);
    }
}
