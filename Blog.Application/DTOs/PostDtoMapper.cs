using Blog.Domain.Aggregates.Author;
using Blog.Domain.Aggregates.Post;

namespace Blog.Application.DTOs;

internal static class PostDtoMapper
{
    public static PostDto ToDto(Post post, AuthorDto? author = null) =>
        new(
            post.PostId.Value,
            post.Title.Value,
            post.Content.Value,
            post.CreatedAtUtc,
            post.AuthorId.Value,
            author);

    public static AuthorDto ToAuthorDto(Author author) =>
        new(
            author.AuthorId.Value,
            author.Name,
            author.Surname,
            author.CreatedAtUtc,
            author.UpdatedAtUtc);

    public static PostDto ToDto(PostWithAuthor postWithAuthor) =>
        ToDto(postWithAuthor.Post, ToAuthorDto(postWithAuthor.Author));
}
