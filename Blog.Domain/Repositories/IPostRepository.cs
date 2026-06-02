using Blog.Domain.Aggregates.Post;

namespace Blog.Domain.Repositories;

public interface IPostRepository
{
    Task AddAsync(Post post, CancellationToken cancellationToken = default);
    Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Post>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Post post, CancellationToken cancellationToken = default);
    Task DeleteAsync(PostId id, CancellationToken cancellationToken = default);
}
