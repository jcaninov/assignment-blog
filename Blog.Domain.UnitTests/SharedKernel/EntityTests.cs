using Blog.Domain.Aggregates.Author;
using Blog.Domain.Aggregates.Post;
using Blog.Domain.SharedKernel;
using NSubstitute;

namespace Blog.Domain.Tests.SharedKernel;

public class EntityTests
{
    private sealed class TestEntity : Entity
    {
        public TestEntity(Guid id) => Id = id;

        public void Raise(IDomainEvent domainEvent) => AddDomainEvent(domainEvent);
    }

    [Fact]
    public void AddDomainEvent_AddsEventToCollection()
    {
        var entity = new TestEntity(Guid.NewGuid());
        var domainEvent = Substitute.For<IDomainEvent>();

        entity.Raise(domainEvent);

        Assert.Single(entity.DomainEvents);
        Assert.Same(domainEvent, entity.DomainEvents[0]);
    }

    [Fact]
    public void ClearDomainEvents_RemovesAllEvents()
    {
        var entity = new TestEntity(Guid.NewGuid());
        entity.Raise(Substitute.For<IDomainEvent>());

        entity.ClearDomainEvents();

        Assert.Empty(entity.DomainEvents);
    }

    [Fact]
    public void Equals_SameId_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var left = Author.Rehydrate(id, "Jane", "Doe", DateTime.UtcNow);
        var right = Author.Rehydrate(id, "Other", "Name", DateTime.UtcNow.AddDays(-1));

        Assert.True(left.Equals(right));
        Assert.True(left == right);
        Assert.False(left != right);
    }

    [Fact]
    public void Equals_DifferentId_ReturnsFalse()
    {
        var left = Author.Rehydrate(Guid.NewGuid(), "Jane", "Doe", DateTime.UtcNow);
        var right = Author.Rehydrate(Guid.NewGuid(), "Jane", "Doe", DateTime.UtcNow);

        Assert.False(left.Equals(right));
        Assert.False(left == right);
        Assert.True(left != right);
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        var entity = Author.Rehydrate(Guid.NewGuid(), "Jane", "Doe", DateTime.UtcNow);

        Assert.False(entity.Equals(null));
        Assert.False(entity == null);
        Assert.True(entity != null);
    }

    [Fact]
    public void OperatorEquals_BothNull_ReturnsTrue()
    {
        Entity? left = null;
        Entity? right = null;

        Assert.True(left == right);
        Assert.False(left != right);
    }

    [Fact]
    public void OperatorEquals_OneNull_ReturnsFalse()
    {
        var entity = Post.Rehydrate(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Title",
            "Description",
            "Content",
            DateTime.UtcNow);

        Assert.False(entity == null);
        Assert.False(null == entity);
        Assert.True(entity != null);
        Assert.True(null != entity);
    }

    [Fact]
    public void GetHashCode_SameId_ReturnsSameHashCode()
    {
        var id = Guid.NewGuid();
        var left = Author.Rehydrate(id, "Jane", "Doe", DateTime.UtcNow);
        var right = Author.Rehydrate(id, "Other", "Name", DateTime.UtcNow);

        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
