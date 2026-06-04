using Dapper;
using Blog.Domain.Repositories;
using Blog.Domain.SharedKernel;

namespace Blog.Infrastructure.Persistence;

public sealed class DomainEventRepository : IDomainEventRepository
{
    private readonly IUnitOfWork _uow;

    public DomainEventRepository(IUnitOfWork uow) => _uow = uow;

    public async Task AddRangeAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        const string sql = @"INSERT INTO public.domain_events(aggregate_type, aggregate_id, event_type, payload, occurred_at, created_at)
                                VALUES (@AggregateType, @AggregateId, @EventType, @Payload::jsonb, @OccurredAt, @CreatedAt)";

        var domainEventList = domainEvents.ToList();
        var parameters = domainEventList.Select(de => new {
            de.AggregateType,
            de.AggregateId,
            de.EventType,
            de.Payload,
            de.OccurredAt,
            CreatedAt = DateTime.UtcNow,
        });

        await _uow.Connection.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }

}