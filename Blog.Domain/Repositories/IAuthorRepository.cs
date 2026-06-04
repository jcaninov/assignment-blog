using Blog.Domain.Aggregates.Author;

namespace Blog.Domain.Repositories;

public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(AuthorId id, CancellationToken cancellationToken = default);
}