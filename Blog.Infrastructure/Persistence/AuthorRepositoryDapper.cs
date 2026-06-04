using System.Data;
using Dapper;
using Blog.Domain.Aggregates.Author;
using Blog.Domain.Repositories;

namespace Blog.Infrastructure.Persistence;

public sealed class AuthorRepositoryDapper : IAuthorRepository
{
    private readonly IUnitOfWork _uow;

    public AuthorRepositoryDapper(IUnitOfWork uow) => _uow = uow;

    public async Task<Author?> GetByIdAsync(AuthorId id, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT id, name, surname, ""createdAt"" FROM author WHERE id = @Id";
        var row = await _uow.Connection.QueryFirstOrDefaultAsync(sql, new { Id = id.Value });
        if (row is null) return null;
        return Author.Rehydrate((Guid)row.id, (string)row.name, (string)row.surname, (DateTime)row.createdAt);
    }
}