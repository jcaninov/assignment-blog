using Blog.Domain.SharedKernel;

namespace Blog.Domain.Aggregates.Post;

public sealed class PostTitle : ValueObject
{
    public const int MaxLength = 200;
    public const int MinLength = 1;

    public string Value { get; }

    public PostTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be empty");

        if (value.Length < MinLength || value.Length > MaxLength)
            throw new ArgumentException($"Title must be between {MinLength} and {MaxLength} characters");

        Value = value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
