using DomainAuthor = Blog.Domain.Aggregates.Author.Author;

namespace Blog.Domain.Tests.Aggregates.Author;

public class AuthorTests
{
    [Fact]
    public void Create_SetsPropertiesAndId()
    {
        var before = DateTime.UtcNow;

        var author = DomainAuthor.Create("Jane", "Doe");

        var after = DateTime.UtcNow;

        Assert.Equal("Jane", author.Name);
        Assert.Equal("Doe", author.Surname);
        Assert.Equal(author.AuthorId.Value, author.Id);
        Assert.InRange(author.CreatedAtUtc, before, after);
        Assert.Empty(author.DomainEvents);
    }

    [Fact]
    public void Rehydrate_RestoresAllFields()
    {
        var id = Guid.NewGuid();
        var createdAt = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);

        var author = DomainAuthor.Rehydrate(id, "Jane", "Doe", createdAt);

        Assert.Equal(id, author.Id);
        Assert.Equal(id, author.AuthorId.Value);
        Assert.Equal("Jane", author.Name);
        Assert.Equal("Doe", author.Surname);
        Assert.Equal(createdAt, author.CreatedAtUtc);
        Assert.Empty(author.DomainEvents);
    }

    [Fact]
    public void Rehydrate_SameId_AreEqual()
    {
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        var left = DomainAuthor.Rehydrate(id, "Jane", "Doe", createdAt);
        var right = DomainAuthor.Rehydrate(id, "Other", "Name", createdAt);

        Assert.True(left == right);
    }
}
