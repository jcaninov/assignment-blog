using Blog.Application.Abstractions;
using Blog.Application.DTOs;
using Blog.Domain.Aggregates.Author;
using Blog.Domain.Aggregates.Post;
using Blog.Domain.Repositories;

namespace Blog.Application.Commands.CreatePost;

public sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, PostDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IPostRepository _postRepository;
    private readonly IDomainEventRepository _domainEventRepository;
    private readonly IAuthorRepository _authorRepository;

    public CreatePostCommandHandler(IUnitOfWork uow, IPostRepository postRepository,
        IDomainEventRepository domainEventRepository, IAuthorRepository authorRepository)
    {
        _uow = uow;
        _postRepository = postRepository;
        _domainEventRepository = domainEventRepository;
        _authorRepository = authorRepository;
    }

    public async Task<PostDto> HandleAsync(CreatePostCommand command, CancellationToken cancellationToken = default)
    {
        var authorId = AuthorId.From(command.AuthorId);
        var author = await _authorRepository.GetByIdAsync(authorId, cancellationToken);
        if (author is null)
        {
            throw new ArgumentException($"Author with id '{command.AuthorId}' was not found.");
        }
        
        _uow.Begin();
        
        var post = Post.Create(authorId, command.Title, command.Description, command.Content);

        try
        {
            await _postRepository.AddAsync(post, cancellationToken);
            await _domainEventRepository.AddRangeAsync(post.DomainEvents, cancellationToken);
            _uow.Commit();
        }
        catch (Exception e)
        {
            _uow.Rollback();
            throw;
        }
        finally
        {
            _uow.Dispose();
        }

        return PostDtoMapper.ToDto(post);
    }
}
