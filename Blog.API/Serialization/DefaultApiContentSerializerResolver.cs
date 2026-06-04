using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Blog.API.Serialization;

public sealed class DefaultApiContentSerializerResolver : IApiContentSerializerResolver
{
    private readonly IReadOnlyDictionary<ApiContentFormat, IApiContentSerializer> _serializers;
    private readonly ApiContentFormat _defaultFormat;

    public DefaultApiContentSerializerResolver(
        IEnumerable<IApiContentSerializer> serializers,
        IOptions<SerializationOptions> options)
    {
        _serializers = serializers.ToDictionary(s => s.Format);
        _defaultFormat = options.Value.DefaultFormat;
    }

    public IApiContentSerializer ResolveForRequest(HttpRequest request)
    {
        var contentType = request.ContentType;
        if (string.IsNullOrWhiteSpace(contentType))
            return GetDefaultSerializer();

        var mediaType = ResolveMediaType(contentType);
        return ResolveByMediaType(mediaType) ?? throw new UnsupportedMediaTypeException(contentType);
    }

    public IApiContentSerializer ResolveForResponse(HttpRequest request)
    {
        if (!request.Headers.TryGetValue(HeaderNames.Accept, out var acceptValues))
            return GetDefaultSerializer();

        var acceptHeader = acceptValues.ToString();
        if (string.IsNullOrWhiteSpace(acceptHeader))
            return GetDefaultSerializer();

        var candidates = ParseAcceptHeader(acceptHeader);
        if (candidates.Count == 0)
            return GetDefaultSerializer();

        var hasExplicitUnsupportedOnly = candidates.All(c =>
            c.MediaType != "*/*" && ResolveByMediaType(c.MediaType) is null);

        foreach (var candidate in candidates)
        {
            if (candidate.MediaType == "*/*")
                return GetDefaultSerializer();

            var serializer = ResolveByMediaType(candidate.MediaType);
            if (serializer is not null)
                return serializer;
        }

        if (hasExplicitUnsupportedOnly)
            throw new NotAcceptableException(acceptHeader);

        return GetDefaultSerializer();
    }

    private IApiContentSerializer GetDefaultSerializer() =>
        _serializers.TryGetValue(_defaultFormat, out var serializer)
            ? serializer
            : throw new InvalidOperationException($"No serializer registered for default format '{_defaultFormat}'.");

    private IApiContentSerializer? ResolveByMediaType(string mediaType) =>
        mediaType switch
        {
            JsonApiContentSerializer.MediaType => _serializers.GetValueOrDefault(ApiContentFormat.Json),
            _ => null
        };

    private static string ResolveMediaType(string contentType)
    {
        if (MediaTypeHeaderValue.TryParse(contentType, out var parsed) && parsed.MediaType.HasValue)
            return parsed.MediaType.Value;

        return contentType.Split(';', 2)[0].Trim();
    }

    private static List<AcceptCandidate> ParseAcceptHeader(string acceptHeader)
    {
        var candidates = new List<AcceptCandidate>();
        foreach (var part in acceptHeader.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var segments = part.Split(';', StringSplitOptions.TrimEntries);
            var mediaType = segments[0];
            var quality = 1.0;

            foreach (var segment in segments.Skip(1))
            {
                if (segment.StartsWith("q=", StringComparison.OrdinalIgnoreCase) &&
                    double.TryParse(segment[2..], System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out var parsedQuality))
                {
                    quality = parsedQuality;
                }
            }

            candidates.Add(new AcceptCandidate(mediaType, quality));
        }

        return candidates
            .OrderByDescending(c => c.Quality)
            .ToList();
    }

    private sealed record AcceptCandidate(string MediaType, double Quality);
}
