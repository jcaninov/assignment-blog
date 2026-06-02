using Blog.Domain.SharedKernel;

namespace Blog.Domain.DomainEvents;

public sealed class PostCreatedEvent : IDomainEvent
{
    public Guid AggregateId { get; }
    public DateTime OccurredAt { get; }
    public string Title { get; }
    public string Content { get; }

    public PostCreatedEvent(Guid aggregateId, string title, string content, DateTime occurredAt)
    {
        AggregateId = aggregateId;
        Title = title;
        Content = content;
        OccurredAt = occurredAt;
    }
}
