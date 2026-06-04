using Blog.API.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Xunit;

namespace Blog.API.UnitTests.Serialization;

public sealed class DefaultApiContentSerializerResolverTests
{
    private readonly DefaultApiContentSerializerResolver _resolver = CreateResolver();

    [Fact]
    public void ResolveForRequest_WithJsonContentType_ReturnsJsonSerializer()
    {
        var context = new DefaultHttpContext();
        context.Request.ContentType = "application/json";

        var serializer = _resolver.ResolveForRequest(context.Request);

        Assert.Equal(ApiContentFormat.Json, serializer.Format);
    }

    [Fact]
    public void ResolveForRequest_WithMissingContentType_ReturnsJsonSerializer()
    {
        var context = new DefaultHttpContext();

        var serializer = _resolver.ResolveForRequest(context.Request);

        Assert.Equal(ApiContentFormat.Json, serializer.Format);
    }

    [Fact]
    public void ResolveForRequest_WithUnsupportedContentType_Throws()
    {
        var context = new DefaultHttpContext();
        context.Request.ContentType = "application/xml";

        Assert.Throws<UnsupportedMediaTypeException>(() => _resolver.ResolveForRequest(context.Request));
    }

    [Fact]
    public void ResolveForResponse_WithMissingAccept_ReturnsJsonSerializer()
    {
        var context = new DefaultHttpContext();

        var serializer = _resolver.ResolveForResponse(context.Request);

        Assert.Equal(ApiContentFormat.Json, serializer.Format);
    }

    [Fact]
    public void ResolveForResponse_WithJsonAccept_ReturnsJsonSerializer()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers.Accept = "application/json";

        var serializer = _resolver.ResolveForResponse(context.Request);

        Assert.Equal(ApiContentFormat.Json, serializer.Format);
    }

    [Fact]
    public void ResolveForResponse_WithUnsupportedAcceptOnly_Throws()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers.Accept = "application/xml";

        Assert.Throws<NotAcceptableException>(() => _resolver.ResolveForResponse(context.Request));
    }

    private static DefaultApiContentSerializerResolver CreateResolver()
    {
        var json = new JsonApiContentSerializer(JsonSerializerOptionsFactory.Create());
        IApiContentSerializer[] serializers = [json];
        return new DefaultApiContentSerializerResolver(
            serializers,
            Options.Create(new SerializationOptions()));
    }
}
