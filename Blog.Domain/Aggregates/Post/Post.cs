using Blog.Domain.DomainEvents;
using Blog.Domain.SharedKernel;

namespace Blog.Domain.Aggregates.Post;

public sealed class Post : Entity
{
    public PostId PostId { get; private set; } = null!;
    public PostTitle Title { get; private set; } = null!;
    public PostContent Content { get; private set; } = null!;
    public DateTime CreatedAtUtc { get; private set; }

    private Post() { }

    public static Post Create(string title, string content)
    {
        var postId = PostId.Create();
        var postTitle = new PostTitle(title);
        var postContent = new PostContent(content);

        var post = new Post
        {
            Id = postId.Value,
            PostId = postId,
            Title = postTitle,
            Content = postContent,
            CreatedAtUtc = DateTime.UtcNow
        };

        post.AddDomainEvent(new PostCreatedEvent(
            postId.Value,
            title,
            content,
            DateTime.UtcNow));

        return post;
    }

    public static Post Rehydrate(Guid id, string title, string content, DateTime createdAtUtc)
    {
        var post = new Post
        {
            Id = id,
            PostId = PostId.From(id),
            Title = new PostTitle(title),
            Content = new PostContent(content),
            CreatedAtUtc = createdAtUtc
        };

        return post;
    }
}
