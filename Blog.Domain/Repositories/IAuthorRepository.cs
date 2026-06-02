using Blog.Domain.Aggregates.Author;

namespace Blog.Domain.Repositories;

public interface IAuthorRepository
{
    Task AddAsync(Author author, CancellationToken cancellationToken = default);
    Task<Author?> GetByIdAsync(AuthorId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Author author, CancellationToken cancellationToken = default);
    Task DeleteAsync(AuthorId id, CancellationToken cancellationToken = default);
}