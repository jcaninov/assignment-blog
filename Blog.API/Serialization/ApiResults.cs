namespace Blog.API.Serialization;

public static class ApiResults
{
    public static IResult Ok(HttpContext http, IApiContentSerializerResolver resolver, object value)
    {
        var serializer = ResolveResponse(http, resolver);
        return new SerializedResult(StatusCodes.Status200OK, serializer, value);
    }

    public static IResult Created(HttpContext http, IApiContentSerializerResolver resolver, string location, object value)
    {
        var serializer = ResolveResponse(http, resolver);
        return new SerializedResult(StatusCodes.Status201Created, serializer, value, location);
    }

    public static IResult BadRequest(HttpContext http, IApiContentSerializerResolver resolver, object value) =>
        Error(http, resolver, StatusCodes.Status400BadRequest, value);

    public static IResult NotFound(HttpContext http, IApiContentSerializerResolver resolver) =>
        Error(http, resolver, StatusCodes.Status404NotFound, new { error = "Resource not found." });

    public static IResult UnsupportedMediaType(HttpContext http, IApiContentSerializerResolver resolver, string mediaType) =>
        Error(http, resolver, StatusCodes.Status415UnsupportedMediaType, new { error = $"Unsupported media type: '{mediaType}'." });

    public static IResult NotAcceptable(HttpContext http, IApiContentSerializerResolver resolver, string acceptHeader) =>
        Error(http, resolver, StatusCodes.Status406NotAcceptable, new { error = $"No supported formatter for Accept: '{acceptHeader}'." });

    private static IResult Error(HttpContext http, IApiContentSerializerResolver resolver, int statusCode, object value)
    {
        var serializer = ResolveResponse(http, resolver);
        return new SerializedResult(statusCode, serializer, value);
    }

    private static IApiContentSerializer ResolveResponse(HttpContext http, IApiContentSerializerResolver resolver)
    {
        try
        {
            return resolver.ResolveForResponse(http.Request);
        }
        catch (NotAcceptableException)
        {
            return resolver.ResolveForRequest(http.Request);
        }
    }
}
