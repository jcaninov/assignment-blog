using Blog.Application.Abstractions;
using Blog.Application.DTOs;
using Blog.Domain.Aggregates.Post;
using Blog.Domain.Repositories;

namespace Blog.Application.Queries.GetPostById;

public sealed class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostDto?>
{
    private readonly IPostRepository _postRepository;

    public GetPostByIdQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<PostDto?> HandleAsync(GetPostByIdQuery query, CancellationToken cancellationToken = default)
    {
        var postId = PostId.From(query.Id);

        if (query.IncludeAuthor)
        {
            var postWithAuthor = await _postRepository.GetByIdWithAuthorAsync(postId, cancellationToken);
            return postWithAuthor is null ? null : PostDtoMapper.ToDto(postWithAuthor);
        }

        var post = await _postRepository.GetByIdAsync(postId, cancellationToken);
        return post is null ? null : PostDtoMapper.ToDto(post);
    }
}
