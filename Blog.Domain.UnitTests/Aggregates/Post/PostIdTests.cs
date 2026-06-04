using Blog.Domain.Aggregates.Post;

namespace Blog.Domain.Tests.Aggregates.Post;

public class PostIdTests
{
    [Fact]
    public void Create_ReturnsNonEmptyGuid()
    {
        var postId = PostId.Create();

        Assert.NotEqual(Guid.Empty, postId.Value);
    }

    [Fact]
    public void From_ValidGuid_ExposesValue()
    {
        var guid = Guid.NewGuid();

        var postId = PostId.From(guid);

        Assert.Equal(guid, postId.Value);
    }

    [Fact]
    public void Constructor_EmptyGuid_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => new PostId(Guid.Empty));

        Assert.Contains("cannot be empty", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void From_EmptyGuid_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => PostId.From(Guid.Empty));

        Assert.Contains("cannot be empty", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Equals_SameValue_AreEqual()
    {
        var guid = Guid.NewGuid();
        var left = PostId.From(guid);
        var right = PostId.From(guid);

        Assert.Equal(left, right);
        Assert.True(left == right);
    }

    [Fact]
    public void Equals_DifferentValue_AreNotEqual()
    {
        var left = PostId.From(Guid.NewGuid());
        var right = PostId.From(Guid.NewGuid());

        Assert.NotEqual(left, right);
        Assert.True(left != right);
    }

    [Fact]
    public void ToString_ReturnsValueString()
    {
        var guid = Guid.NewGuid();
        var postId = PostId.From(guid);

        Assert.Equal(guid.ToString(), postId.ToString());
    }
}
