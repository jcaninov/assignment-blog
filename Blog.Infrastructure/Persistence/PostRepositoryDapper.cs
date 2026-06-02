using System.Data;
using Dapper;
using Blog.Domain.Aggregates.Post;
using Blog.Domain.Repositories;

namespace Blog.Infrastructure.Persistence;

public sealed class PostRepositoryDapper : IPostRepository
{
    private readonly IDbConnection _db;

    public PostRepositoryDapper(IDbConnection db) => _db = db;

    public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
    {
        const string sql = @"INSERT INTO post (id, author_id, title, description, content, ""createdAt"", ""updatedAt"")
VALUES (@Id, @AuthorId, @Title, @Description, @Content, @CreatedAt, @UpdatedAt)";

        await _db.ExecuteAsync(new CommandDefinition(sql, new {
            Id = post.PostId.Value,
            AuthorId = (Guid?)null,
            Title = post.Title.Value,
            Description = (string?)null,
            Content = post.Content.Value,
            CreatedAt = post.CreatedAtUtc,
            UpdatedAt = post.CreatedAtUtc
        }, cancellationToken: cancellationToken));
    }

    public async Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT id, title, description, content, ""createdAt"", ""updatedAt"" FROM post WHERE id = @Id";
        var row = await _db.QueryFirstOrDefaultAsync(sql, new { Id = id.Value });
        if (row is null) return null;
        return Post.Rehydrate((Guid)row.id, (string)row.title, (string)row.content, (DateTime)row.createdAt);
    }

    public async Task<IReadOnlyList<Post>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT id, title, description, content, ""createdAt"", ""updatedAt"" FROM post";
        var rows = await _db.QueryAsync(sql);
        var list = rows.Select(r => Post.Rehydrate((Guid)r.id, (string)r.title, (string)r.content, (DateTime)r.createdAt)).ToList();
        return list;
    }

    public async Task UpdateAsync(Post post, CancellationToken cancellationToken = default)
    {
        const string sql = @"UPDATE post SET title = @Title, description = @Description, content = @Content, ""updatedAt"" = @UpdatedAt WHERE id = @Id";
        await _db.ExecuteAsync(new CommandDefinition(sql, new {
            Id = post.PostId.Value,
            Title = post.Title.Value,
            Description = (string?)null,
            Content = post.Content.Value,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(PostId id, CancellationToken cancellationToken = default)
    {
        const string sql = @"DELETE FROM post WHERE id = @Id";
        await _db.ExecuteAsync(new CommandDefinition(sql, new { Id = id.Value }, cancellationToken: cancellationToken));
    }
}