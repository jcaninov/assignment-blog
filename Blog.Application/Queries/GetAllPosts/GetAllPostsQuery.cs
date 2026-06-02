using Blog.Application.Abstractions;
using Blog.Application.DTOs;

namespace Blog.Application.Queries.GetAllPosts;

public sealed class GetAllPostsQuery : IQuery<IReadOnlyList<PostDto>>
{
}
