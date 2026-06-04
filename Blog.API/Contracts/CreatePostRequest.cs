namespace Blog.API.Contracts;

public record CreatePostRequest(Guid AuthorId, string Title, string Description, string Content);
