using Blog.Domain.SharedKernel;

namespace Blog.Domain.Aggregates.Author;

public sealed class Author : Entity
{
    public AuthorId AuthorId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    private Author() { }

    public static Author Create(string name, string surname)
    {
        var id = AuthorId.Create();
        var author = new Author
        {
            Id = id.Value,
            AuthorId = id,
            Name = name,
            Surname = surname,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        return author;
    }

    public static Author Rehydrate(Guid id, string name, string surname, DateTime createdAtUtc, DateTime updatedAtUtc)
    {
        var a = new Author
        {
            Id = id,
            AuthorId = AuthorId.From(id),
            Name = name,
            Surname = surname,
            CreatedAtUtc = createdAtUtc,
            UpdatedAtUtc = updatedAtUtc
        };

        return a;
    }
}