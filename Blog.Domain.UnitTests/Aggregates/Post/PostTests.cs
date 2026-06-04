using Blog.Domain.Aggregates.Author;
using Blog.Domain.DomainEvents;
using PostAggregate = Blog.Domain.Aggregates.Post.Post;

namespace Blog.Domain.Tests.Aggregates.Post;

public class PostTests
{
    private static AuthorId ValidAuthorId() => AuthorId.From(Guid.NewGuid());

    private const string ValidTitle = "Title";
    private const string ValidDescription = "Description";
    private const string ValidContent = "Content";

    [Fact]
    public void Create_SetsPropertiesAndRaisesDomainEvent()
    {
        var authorId = ValidAuthorId();
        var before = DateTime.UtcNow;

        var post = PostAggregate.Create(authorId, ValidTitle, ValidDescription, ValidContent);

        var after = DateTime.UtcNow;

        Assert.Equal(authorId, post.AuthorId);
        Assert.Equal(ValidTitle, post.Title.Value);
        Assert.Equal(ValidDescription, post.Description.Value);
        Assert.Equal(ValidContent, post.Content.Value);
        Assert.Equal(post.PostId.Value, post.Id);
        Assert.InRange(post.CreatedAtUtc, before, after);

        var domainEvent = Assert.Single(post.DomainEvents);
        Assert.IsType<PostCreatedEvent>(domainEvent);
    }

    [Fact]
    public void ClearDomainEvents_RemovesEvents()
    {
        var post = PostAggregate.Create(ValidAuthorId(), ValidTitle, ValidDescription, ValidContent);

        post.ClearDomainEvents();

        Assert.Empty(post.DomainEvents);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidTitle_ThrowsArgumentException(string title)
    {
        Assert.Throws<ArgumentException>(() =>
            PostAggregate.Create(ValidAuthorId(), title, ValidDescription, ValidContent));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidDescription_ThrowsArgumentException(string description)
    {
        Assert.Throws<ArgumentException>(() =>
            PostAggregate.Create(ValidAuthorId(), ValidTitle, description, ValidContent));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidContent_ThrowsArgumentException(string content)
    {
        Assert.Throws<ArgumentException>(() =>
            PostAggregate.Create(ValidAuthorId(), ValidTitle, ValidDescription, content));
    }

    [Fact]
    public void Rehydrate_RestoresStateWithoutDomainEvents()
    {
        var id = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var createdAt = new DateTime(2024, 6, 1, 8, 30, 0, DateTimeKind.Utc);

        var post = PostAggregate.Rehydrate(
            id,
            authorId,
            ValidTitle,
            ValidDescription,
            ValidContent,
            createdAt);

        Assert.Equal(id, post.Id);
        Assert.Equal(id, post.PostId.Value);
        Assert.Equal(authorId, post.AuthorId.Value);
        Assert.Equal(ValidTitle, post.Title.Value);
        Assert.Equal(ValidDescription, post.Description.Value);
        Assert.Equal(ValidContent, post.Content.Value);
        Assert.Equal(createdAt, post.CreatedAtUtc);
        Assert.Empty(post.DomainEvents);
    }

    [Fact]
    public void Rehydrate_SameId_AreEqual()
    {
        var id = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        var left = PostAggregate.Rehydrate(id, authorId, ValidTitle, ValidDescription, ValidContent, createdAt);
        var right = PostAggregate.Rehydrate(id, Guid.NewGuid(), "Other", "Other", "Other", createdAt.AddDays(-1));

        Assert.True(left == right);
    }
}
