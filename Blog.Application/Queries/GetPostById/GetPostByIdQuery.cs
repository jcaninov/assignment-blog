using Blog.Application.Abstractions;
using Blog.Application.DTOs;

namespace Blog.Application.Queries.GetPostById;

public sealed class GetPostByIdQuery : IQuery<PostDto?>
{
    public Guid Id { get; }
    public bool IncludeAuthor { get; }

    public GetPostByIdQuery(Guid id, bool includeAuthor)
    {
        Id = id;
        IncludeAuthor = includeAuthor;
    }
}
