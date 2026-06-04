using Blog.Domain.Aggregates.Post;

namespace Blog.Domain.Repositories;

public interface IPostRepository
{
    Task AddAsync(Post post, CancellationToken cancellationToken = default);
    Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken = default);
    Task<PostWithAuthor?> GetByIdWithAuthorAsync(PostId id, CancellationToken cancellationToken = default);
}
