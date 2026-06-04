namespace Blog.Domain.SharedKernel;

public interface IDomainEvent
{
    string AggregateType { get; }
    string EventType { get; }
    Guid AggregateId { get; }
    DateTimeOffset OccurredAt { get; }
    string Payload { get; }
}
