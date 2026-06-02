using Blog.Application.Abstractions;

namespace Blog.Application.Commands.CreatePost;

public sealed class CreatePostCommand : ICommand
{
    public string Title { get; }
    public string Content { get; }

    public CreatePostCommand(string title, string content)
    {
        Title = title;
        Content = content;
    }
}
