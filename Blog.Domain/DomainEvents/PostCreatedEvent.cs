using System.Text.Json;
using Blog.Domain.Aggregates.Post;
using Blog.Domain.SharedKernel;

namespace Blog.Domain.DomainEvents;

public sealed record PostCreatedEvent : IDomainEvent
{
    public string AggregateType => "blog.post";
    public string EventType => "blog.post.created";
    public Guid AggregateId { get; }
    public DateTimeOffset OccurredAt { get; }
    public string Payload { get; }

    public PostCreatedEvent(Post post, DateTimeOffset occurredAt)
    {
        AggregateId = post.PostId.Value;
        OccurredAt = occurredAt;
        Payload = JsonSerializer.Serialize(post);
    }
}
