namespace Blog.API.Serialization;

public sealed class NotAcceptableException : Exception
{
    public NotAcceptableException(string acceptHeader)
        : base($"No supported formatter for Accept: '{acceptHeader}'.")
    {
        AcceptHeader = acceptHeader;
    }

    public string AcceptHeader { get; }
}
