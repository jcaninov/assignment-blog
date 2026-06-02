using Blog.Domain.SharedKernel;

namespace Blog.Domain.Aggregates.Post;

public sealed class PostId : ValueObject
{
    public Guid Value { get; }

    public PostId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Post ID cannot be empty");

        Value = value;
    }

    public static PostId Create() => new(Guid.NewGuid());
    public static PostId From(Guid value) => new(value);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
