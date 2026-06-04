using System.Data;
using Dapper;
using Blog.Domain.Aggregates.Author;
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
            AuthorId = post.AuthorId.Value,
            Title = post.Title.Value,
            Description = (string?)null,
            Content = post.Content.Value,
            CreatedAt = post.CreatedAtUtc,
            UpdatedAt = post.CreatedAtUtc
        }, cancellationToken: cancellationToken));
    }

    public async Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT id, author_id, title, content, ""createdAt"" FROM post WHERE id = @Id";
        var row = await _db.QueryFirstOrDefaultAsync(
            new CommandDefinition(sql, new { Id = id.Value }, cancellationToken: cancellationToken));
        if (row is null) return null;
        return Post.Rehydrate(
            (Guid)row.id,
            (Guid)row.author_id,
            (string)row.title,
            (string)row.content,
            (DateTime)row.createdAt);
    }

    public async Task<PostWithAuthor?> GetByIdWithAuthorAsync(PostId id, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT p.id, p.author_id, p.title, p.content, p.""createdAt"",
       a.id AS author_row_id, a.name, a.surname, a.""createdAt"" AS author_created_at, a.""updatedAt"" AS author_updated_at
FROM post p
INNER JOIN author a ON a.id = p.author_id
WHERE p.id = @Id";

        var row = await _db.QueryFirstOrDefaultAsync(
            new CommandDefinition(sql, new { Id = id.Value }, cancellationToken: cancellationToken));
        if (row is null) return null;

        var post = Post.Rehydrate(
            (Guid)row.id,
            (Guid)row.author_id,
            (string)row.title,
            (string)row.content,
            (DateTime)row.createdAt);

        var author = Author.Rehydrate(
            (Guid)row.author_row_id,
            (string)row.name,
            (string)row.surname,
            (DateTime)row.author_created_at,
            (DateTime)row.author_updated_at);

        return new PostWithAuthor(post, author);
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
