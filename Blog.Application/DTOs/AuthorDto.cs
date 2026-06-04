namespace Blog.Application.DTOs;

public record AuthorDto(Guid Id, string Name, string Surname, DateTime CreatedAtUtc);
