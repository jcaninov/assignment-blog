using System.Text.Json;
using Blog.Domain.Aggregates.Author;
using Blog.Domain.DomainEvents;
using Blog.Domain.SharedKernel;
using PostAggregate = Blog.Domain.Aggregates.Post.Post;

namespace Blog.Domain.Tests.DomainEvents;

public class PostCreatedEventTests
{
    [Fact]
    public void Constructor_SetsMetadataAndPayload()
    {
        var authorId = AuthorId.From(Guid.NewGuid());
        var post = PostAggregate.Create(authorId, "My Title", "My Description", "My Content");
        var occurredAt = new DateTimeOffset(2024, 3, 10, 14, 0, 0, TimeSpan.Zero);

        var domainEvent = new PostCreatedEvent(post, occurredAt);

        Assert.Equal("blog.post", domainEvent.AggregateType);
        Assert.Equal("blog.post.created", domainEvent.EventType);
        Assert.Equal(post.PostId.Value, domainEvent.AggregateId);
        Assert.Equal(occurredAt, domainEvent.OccurredAt);
        Assert.False(string.IsNullOrWhiteSpace(domainEvent.Payload));

        using var document = JsonDocument.Parse(domainEvent.Payload);
        var root = document.RootElement;
        Assert.Equal("My Title", root.GetProperty("Title").GetProperty("Value").GetString());
        Assert.IsAssignableFrom<IDomainEvent>(domainEvent);
    }
}
