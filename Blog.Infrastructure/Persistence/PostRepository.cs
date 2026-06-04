using Blog.Domain.Aggregates.Author;
using Blog.Domain.Aggregates.Post;
using Blog.Domain.Repositories;

namespace Blog.Infrastructure.Persistence;

public sealed class PostRepository : IPostRepository
{
    private readonly List<Post> _posts = new List<Post>();
    private readonly IAuthorRepository _authorRepository;

    public PostRepository(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

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

    public async Task<PostWithAuthor?> GetByIdWithAuthorAsync(PostId id, CancellationToken cancellationToken = default)
    {
        var post = _posts.FirstOrDefault(p => p.PostId == id);
        if (post is null) return null;

        var author = await _authorRepository.GetByIdAsync(post.AuthorId, cancellationToken);
        if (author is null) return null;

        return new PostWithAuthor(post, author);
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
