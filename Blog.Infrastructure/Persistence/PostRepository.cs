using Blog.Domain.Aggregates.Post;
using Blog.Domain.Repositories;

namespace Blog.Infrastructure.Persistence;

public sealed class PostRepository : IPostRepository
{
    private readonly List<Post> _posts = new List<Post>();

    public Task AddAsync(Post post, CancellationToken cancellationToken = default)
    {
        _posts.Add(post);
        return Task.CompletedTask;
    }

    public Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken = default)
    {
        var post = _posts.FirstOrDefault(p => p.PostId == id);
        return Task.FromResult(post);
    }

    public Task<IReadOnlyList<Post>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Post>>(_posts.AsReadOnly());
    }

    public Task UpdateAsync(Post post, CancellationToken cancellationToken = default)
    {
        var existingPost = _posts.FirstOrDefault(p => p.PostId == post.PostId);
        if (existingPost is not null)
        {
            var index = _posts.IndexOf(existingPost);
            _posts[index] = post;
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(PostId id, CancellationToken cancellationToken = default)
    {
        var post = _posts.FirstOrDefault(p => p.PostId == id);
        if (post is not null)
        {
            _posts.Remove(post);
        }

        return Task.CompletedTask;
    }
}
