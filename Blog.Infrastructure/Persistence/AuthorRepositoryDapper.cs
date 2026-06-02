using System.Data;
using Dapper;
using Blog.Domain.Aggregates.Author;
using Blog.Domain.Repositories;

namespace Blog.Infrastructure.Persistence;

public sealed class AuthorRepositoryDapper : IAuthorRepository
{
    private readonly IDbConnection _db;

    public AuthorRepositoryDapper(IDbConnection db) => _db = db;

    public async Task AddAsync(Author author, CancellationToken cancellationToken = default)
    {
        const string sql = @"INSERT INTO author (id, name, surname, ""createdAt"", ""updatedAt"")
VALUES (@Id, @Name, @Surname, @CreatedAt, @UpdatedAt)";

        await _db.ExecuteAsync(new CommandDefinition(sql, new {
            Id = author.AuthorId.Value,
            Name = author.Name,
            Surname = author.Surname,
            CreatedAt = author.CreatedAtUtc,
            UpdatedAt = author.UpdatedAtUtc
        }, cancellationToken: cancellationToken));
    }

    public async Task<Author?> GetByIdAsync(AuthorId id, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT id, name, surname, ""createdAt"", ""updatedAt"" FROM author WHERE id = @Id";
        var row = await _db.QueryFirstOrDefaultAsync(sql, new { Id = id.Value });
        if (row is null) return null;
        return Author.Rehydrate((Guid)row.id, (string)row.name, (string)row.surname, (DateTime)row.createdAt, (DateTime)row.updatedAt);
    }

    public async Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT id, name, surname, ""createdAt"", ""updatedAt"" FROM author";
        var rows = await _db.QueryAsync(sql);
        var list = rows.Select(r => Author.Rehydrate((Guid)r.id, (string)r.name, (string)r.surname, (DateTime)r.createdAt, (DateTime)r.updatedAt)).ToList();
        return list;
    }

    public async Task UpdateAsync(Author author, CancellationToken cancellationToken = default)
    {
        const string sql = @"UPDATE author SET name = @Name, surname = @Surname, ""updatedAt"" = @UpdatedAt WHERE id = @Id";
        await _db.ExecuteAsync(new CommandDefinition(sql, new {
            Id = author.AuthorId.Value,
            Name = author.Name,
            Surname = author.Surname,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(AuthorId id, CancellationToken cancellationToken = default)
    {
        const string sql = @"DELETE FROM author WHERE id = @Id";
        await _db.ExecuteAsync(new CommandDefinition(sql, new { Id = id.Value }, cancellationToken: cancellationToken));
    }
}