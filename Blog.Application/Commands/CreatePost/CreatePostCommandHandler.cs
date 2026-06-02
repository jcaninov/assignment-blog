using Blog.Application.Abstractions;
using Blog.Application.DTOs;
using Blog.Domain.Aggregates.Post;
using Blog.Domain.Repositories;

namespace Blog.Application.Commands.CreatePost;

public sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, PostDto>
{
    private readonly IPostRepository _postRepository;

    public CreatePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<PostDto> HandleAsync(CreatePostCommand command, CancellationToken cancellationToken = default)
    {
        var post = Post.Create(command.Title, command.Content);
        await _postRepository.AddAsync(post, cancellationToken);

        return new PostDto(
            post.PostId.Value,
            post.Title.Value,
            post.Content.Value,
            post.CreatedAtUtc);
    }
}
