using System.Data;
using Dapper;
using Blog.Domain.Aggregates.Author;
using Blog.Domain.Aggregates.Post;
using Blog.Domain.Repositories;

namespace Blog.Infrastructure.Persistence;

public sealed class PostRepositoryDapper : IPostRepository
{
    private readonly IUnitOfWork _uow;

    public PostRepositoryDapper(IUnitOfWork uow) => _uow = uow;

    public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
    {
        const string sql = @"INSERT INTO post (id, author_id, title, description, content, ""createdAt"")
                                VALUES (@Id, @AuthorId, @Title, @Description, @Content, @CreatedAt)";

        await _uow.Connection.ExecuteAsync(new CommandDefinition(sql, new {
            Id = post.PostId.Value,
            AuthorId = post.AuthorId.Value,
            Title = post.Title.Value,
            Description = post.Description.Value,
            Content = post.Content.Value,
            CreatedAt = post.CreatedAtUtc
        }, cancellationToken: cancellationToken));
    }

    public async Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT id, author_id, title, description, content, ""createdAt"" FROM post WHERE id = @Id";
        var row = await _uow.Connection.QueryFirstOrDefaultAsync(
            new CommandDefinition(sql, new { Id = id.Value }, cancellationToken: cancellationToken));
        if (row is null) return null;
        return Post.Rehydrate(
            (Guid)row.id,
            (Guid)row.author_id,
            (string)row.title,
            (string)row.description,
            (string)row.content,
            (DateTime)row.createdAt);
    }

    public async Task<PostWithAuthor?> GetByIdWithAuthorAsync(PostId id, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT p.id, p.author_id, p.title, p.description, p.content, p.""createdAt"",
       a.id AS author_row_id, a.name, a.surname, a.""createdAt"" AS author_created_at
FROM post p
INNER JOIN author a ON a.id = p.author_id
WHERE p.id = @Id";

        var row = await _uow.Connection.QueryFirstOrDefaultAsync(
            new CommandDefinition(sql, new { Id = id.Value }, cancellationToken: cancellationToken));
        if (row is null) return null;

        var post = Post.Rehydrate(
            (Guid)row.id,
            (Guid)row.author_id,
            (string)row.title,
            (string)row.description,
            (string)row.content,
            (DateTime)row.createdAt);

        var author = Author.Rehydrate(
            (Guid)row.author_row_id,
            (string)row.name,
            (string)row.surname,
            (DateTime)row.author_created_at);

        return new PostWithAuthor(post, author);
    }
}
