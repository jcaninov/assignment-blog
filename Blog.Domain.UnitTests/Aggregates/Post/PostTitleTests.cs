using Blog.Domain.Aggregates.Post;

namespace Blog.Domain.Tests.Aggregates.Post;

public class PostTitleTests
{
    [Fact]
    public void Constructor_MinLength_Succeeds()
    {
        var title = new PostTitle("x");

        Assert.Equal("x", title.Value);
    }

    [Fact]
    public void Constructor_MaxLength_Succeeds()
    {
        var value = new string('x', PostTitle.MaxLength);

        var title = new PostTitle(value);

        Assert.Equal(value, title.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyOrWhitespace_ThrowsArgumentException(string value)
    {
        var ex = Assert.Throws<ArgumentException>(() => new PostTitle(value));

        Assert.Contains("empty", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_TooLong_ThrowsArgumentException()
    {
        var value = new string('x', PostTitle.MaxLength + 1);

        var ex = Assert.Throws<ArgumentException>(() => new PostTitle(value));

        Assert.Contains(PostTitle.MinLength.ToString(), ex.Message);
        Assert.Contains(PostTitle.MaxLength.ToString(), ex.Message);
    }

    [Fact]
    public void Equals_SameValue_AreEqual()
    {
        var left = new PostTitle("My Title");
        var right = new PostTitle("My Title");

        Assert.Equal(left, right);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var title = new PostTitle("My Title");

        Assert.Equal("My Title", title.ToString());
    }
}
