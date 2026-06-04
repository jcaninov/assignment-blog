namespace Blog.API.Serialization;

public interface IApiContentSerializer
{
    ApiContentFormat Format { get; }

    string ContentType { get; }

    ValueTask<T?> DeserializeAsync<T>(Stream body, CancellationToken cancellationToken = default);

    ValueTask SerializeAsync<T>(Stream response, T value, CancellationToken cancellationToken = default);
}
