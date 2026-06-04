using Blog.API.Contracts;
using Blog.API.Serialization;
using Xunit;

namespace Blog.API.UnitTests.Serialization;

public sealed class JsonApiContentSerializerTests
{
    private readonly JsonApiContentSerializer _serializer = new(JsonSerializerOptionsFactory.Create());

    [Fact]
    public async Task RoundTrips_CreatePostRequest()
    {
        var original = new CreatePostRequest(Guid.NewGuid(), "Title", "Description", "Content");

        await using var writeStream = new MemoryStream();
        await _serializer.SerializeAsync(writeStream, original);
        writeStream.Position = 0;

        var deserialized = await _serializer.DeserializeAsync<CreatePostRequest>(writeStream);
        Assert.NotNull(deserialized);
        Assert.Equal(original, deserialized);
    }

    [Fact]
    public async Task DeserializeAsync_OnNonSeekableStream_DoesNotThrow()
    {
        var json = """{"authorId":"11111111-1111-1111-1111-111111111111","title":"T","description":"D","content":"C"}""";
        await using var inner = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        await using var body = new NonSeekableStream(inner);

        var result = await _serializer.DeserializeAsync<CreatePostRequest>(body);

        Assert.NotNull(result);
        Assert.Equal("T", result.Title);
    }

    private sealed class NonSeekableStream(Stream inner) : Stream
    {
        public override bool CanSeek => false;
        public override bool CanRead => inner.CanRead;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() => inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) => inner.Read(buffer, offset, count);
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
