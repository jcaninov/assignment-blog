using Blog.API.Serialization;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Blog.API.UnitTests.Serialization;

public sealed class SerializedResultTests
{
    [Fact]
    public async Task ExecuteAsync_SetsStatusContentTypeAndBody()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var serializer = new JsonApiContentSerializer(JsonSerializerOptionsFactory.Create());
        var result = new SerializedResult(
            StatusCodes.Status200OK,
            serializer,
            new { message = "hello" });

        await result.ExecuteAsync(context);

        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);

        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        var body = await reader.ReadToEndAsync();
        Assert.Contains("hello", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExecuteAsync_Created_SetsLocationHeader()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var serializer = new JsonApiContentSerializer(JsonSerializerOptionsFactory.Create());
        var result = new SerializedResult(
            StatusCodes.Status201Created,
            serializer,
            new { id = Guid.Empty },
            "/posts/123");

        await result.ExecuteAsync(context);

        Assert.Equal("/posts/123", context.Response.Headers.Location.ToString());
    }
}
