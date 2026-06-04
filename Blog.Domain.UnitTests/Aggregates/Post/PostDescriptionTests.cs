using Blog.Domain.Aggregates.Post;

namespace Blog.Domain.Tests.Aggregates.Post;

public class PostDescriptionTests
{
    [Fact]
    public void Constructor_MinLength_Succeeds()
    {
        var description = new PostDescription("x");

        Assert.Equal("x", description.Value);
    }

    [Fact]
    public void Constructor_MaxLength_Succeeds()
    {
        var value = new string('x', PostDescription.MaxLength);

        var description = new PostDescription(value);

        Assert.Equal(value, description.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_EmptyOrWhitespace_ThrowsArgumentException(string value)
    {
        var ex = Assert.Throws<ArgumentException>(() => new PostDescription(value));

        Assert.Contains("empty", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_TooLong_ThrowsArgumentException()
    {
        var value = new string('x', PostDescription.MaxLength + 1);

        var ex = Assert.Throws<ArgumentException>(() => new PostDescription(value));

        Assert.Contains(PostDescription.MinLength.ToString(), ex.Message);
        Assert.Contains(PostDescription.MaxLength.ToString(), ex.Message);
    }

    [Fact]
    public void Equals_SameValue_AreEqual()
    {
        var left = new PostDescription("Summary");
        var right = new PostDescription("Summary");

        Assert.Equal(left, right);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var description = new PostDescription("Summary");

        Assert.Equal("Summary", description.ToString());
    }
}
