namespace Blog.API.Serialization;

public sealed class UnsupportedMediaTypeException : Exception
{
    public UnsupportedMediaTypeException(string mediaType)
        : base($"Unsupported media type: '{mediaType}'.")
    {
        MediaType = mediaType;
    }

    public string MediaType { get; }
}
