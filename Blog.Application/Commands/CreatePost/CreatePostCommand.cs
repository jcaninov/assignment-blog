using Blog.Application.Abstractions;

namespace Blog.Application.Commands.CreatePost;

public sealed class CreatePostCommand : ICommand
{
    public Guid AuthorId { get; }
    public string Title { get; }
    public string Content { get; }

    public CreatePostCommand(Guid authorId, string title, string content)
    {
        AuthorId = authorId;
        Title = title;
        Content = content;
    }
}
