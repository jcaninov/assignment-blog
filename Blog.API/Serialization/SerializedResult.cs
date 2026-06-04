namespace Blog.API.Serialization;

public sealed class SerializedResult : IResult
{
    private readonly int _statusCode;
    private readonly object? _value;
    private readonly string? _location;
    private readonly IApiContentSerializer _serializer;

    public SerializedResult(
        int statusCode,
        IApiContentSerializer serializer,
        object? value,
        string? location = null)
    {
        _statusCode = statusCode;
        _serializer = serializer;
        _value = value;
        _location = location;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = _statusCode;
        httpContext.Response.ContentType = _serializer.ContentType;

        if (_location is not null)
            httpContext.Response.Headers.Location = _location;

        if (_value is null || _statusCode == StatusCodes.Status204NoContent)
            return;

        await _serializer.SerializeAsync(httpContext.Response.Body, _value, httpContext.RequestAborted);
    }
}
