using Blog.Application.Abstractions;
using Blog.Application.DTOs;
using Blog.Domain.Aggregates.Author;
using Blog.Domain.Aggregates.Post;
using Blog.Domain.Repositories;

namespace Blog.Application.Commands.CreatePost;

public sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, PostDto>
{
    private readonly IPostRepository _postRepository;
    private readonly IAuthorRepository _authorRepository;

    public CreatePostCommandHandler(IPostRepository postRepository, IAuthorRepository authorRepository)
    {
        _postRepository = postRepository;
        _authorRepository = authorRepository;
    }

    public async Task<PostDto> HandleAsync(CreatePostCommand command, CancellationToken cancellationToken = default)
    {
        var authorId = AuthorId.From(command.AuthorId);
        var author = await _authorRepository.GetByIdAsync(authorId, cancellationToken);
        if (author is null)
            throw new ArgumentException($"Author with id '{command.AuthorId}' was not found.");

        var post = Post.Create(authorId, command.Title, command.Content);
        await _postRepository.AddAsync(post, cancellationToken);

        return PostDtoMapper.ToDto(post);
    }
}
