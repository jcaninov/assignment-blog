using Blog.Domain.SharedKernel;

namespace Blog.Domain.Repositories;

public interface IDomainEventRepository
{
    Task AddRangeAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}