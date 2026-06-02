using Blog.Domain.SharedKernel;

namespace Blog.Infrastructure.DomainEventHandling;

public interface IDomainEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}

public sealed class DomainEventPublisher : IDomainEventPublisher
{
    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // For now, just log the event. In production, you'd integrate with MediatR, RabbitMQ, etc.
        return Task.CompletedTask;
    }
}
