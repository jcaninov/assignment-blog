namespace Blog.Domain.SharedKernel;

public interface IDomainEvent
{
    Guid AggregateId { get; }
    DateTime OccurredAt { get; }
}
