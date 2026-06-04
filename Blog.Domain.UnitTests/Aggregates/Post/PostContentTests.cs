using Blog.Domain.Aggregates.Post;

namespace Blog.Domain.Tests.Aggregates.Post;

public class PostContentTests
{
    [Fact]
    public void Constructor_MinLength_Succeeds()
    {
        var content = new PostContent("x");

        Assert.Equal("x", content.Value);
    }

    [Fact]
    public void Constructor_MaxLength_Succeeds()
    {
        var value = new string('x', PostContent.MaxLength);

        var content = new PostContent(value);

        Assert.Equal(value, content.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyOrWhitespace_ThrowsArgumentException(string value)
    {
        var ex = Assert.Throws<ArgumentException>(() => new PostContent(value));

        Assert.Contains("empty", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_TooLong_ThrowsArgumentException()
    {
        var value = new string('x', PostContent.MaxLength + 1);

        var ex = Assert.Throws<ArgumentException>(() => new PostContent(value));

        Assert.Contains(PostContent.MinLength.ToString(), ex.Message);
        Assert.Contains(PostContent.MaxLength.ToString(), ex.Message);
    }

    [Fact]
    public void Equals_SameValue_AreEqual()
    {
        var left = new PostContent("Body");
        var right = new PostContent("Body");

        Assert.Equal(left, right);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var content = new PostContent("Body");

        Assert.Equal("Body", content.ToString());
    }
}
