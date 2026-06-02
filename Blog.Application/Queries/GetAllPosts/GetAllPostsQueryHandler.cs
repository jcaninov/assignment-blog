using Blog.Application.Abstractions;
using Blog.Application.DTOs;
using Blog.Domain.Repositories;

namespace Blog.Application.Queries.GetAllPosts;

public sealed class GetAllPostsQueryHandler : IQueryHandler<GetAllPostsQuery, IReadOnlyList<PostDto>>
{
    private readonly IPostRepository _postRepository;

    public GetAllPostsQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IReadOnlyList<PostDto>> HandleAsync(GetAllPostsQuery query, CancellationToken cancellationToken = default)
    {
        var posts = await _postRepository.GetAllAsync(cancellationToken);

        return posts
            .Select(p => new PostDto(
                p.PostId.Value,
                p.Title.Value,
                p.Content.Value,
                p.CreatedAtUtc))
            .ToList()
            .AsReadOnly();
    }
}
