namespace Blog.API.Serialization;

public interface IApiContentSerializerResolver
{
    IApiContentSerializer ResolveForRequest(HttpRequest request);

    IApiContentSerializer ResolveForResponse(HttpRequest request);
}
