using Blog.Domain.Aggregates.Author;

namespace Blog.Domain.Tests.Aggregates.Author;

public class AuthorIdTests
{
    [Fact]
    public void Create_ReturnsNonEmptyGuid()
    {
        var authorId = AuthorId.Create();

        Assert.NotEqual(Guid.Empty, authorId.Value);
    }

    [Fact]
    public void From_ValidGuid_ExposesValue()
    {
        var guid = Guid.NewGuid();

        var authorId = AuthorId.From(guid);

        Assert.Equal(guid, authorId.Value);
    }

    [Fact]
    public void Constructor_EmptyGuid_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => new AuthorId(Guid.Empty));

        Assert.Contains("cannot be empty", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void From_EmptyGuid_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => AuthorId.From(Guid.Empty));

        Assert.Contains("cannot be empty", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Equals_SameValue_AreEqual()
    {
        var guid = Guid.NewGuid();
        var left = AuthorId.From(guid);
        var right = AuthorId.From(guid);

        Assert.Equal(left, right);
        Assert.True(left == right);
    }

    [Fact]
    public void Equals_DifferentValue_AreNotEqual()
    {
        var left = AuthorId.From(Guid.NewGuid());
        var right = AuthorId.From(Guid.NewGuid());

        Assert.NotEqual(left, right);
        Assert.True(left != right);
    }

    [Fact]
    public void ToString_ReturnsValueString()
    {
        var guid = Guid.NewGuid();
        var authorId = AuthorId.From(guid);

        Assert.Equal(guid.ToString(), authorId.ToString());
    }
}
