namespace Blog.Application.DTOs;

public record PostDto(
    Guid Id,
    string Title,
    string Description,
    string Content,
    DateTime CreatedAtUtc,
    Guid AuthorId,
    AuthorDto? Author);
