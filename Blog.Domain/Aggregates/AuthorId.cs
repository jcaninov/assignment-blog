namespace Blog.Domain.Aggregates.Author;

using Blog.Domain.SharedKernel;

public sealed class AuthorId : ValueObject
{
    public Guid Value { get; }

    public AuthorId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Author ID cannot be empty");

        Value = value;
    }

    public static AuthorId Create() => new(Guid.NewGuid());
    public static AuthorId From(Guid value) => new(value);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}