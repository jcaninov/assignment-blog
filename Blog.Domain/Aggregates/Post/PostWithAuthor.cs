using Blog.Domain.Aggregates.Author;

namespace Blog.Domain.Aggregates.Post;

public sealed record PostWithAuthor(Post Post, Author.Author Author);
