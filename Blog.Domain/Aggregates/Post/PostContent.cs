using Blog.Domain.SharedKernel;

namespace Blog.Domain.Aggregates.Post;

public sealed class PostContent : ValueObject
{
    public const int MaxLength = 5000;
    public const int MinLength = 1;

    public string Value { get; }

    public PostContent(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Content cannot be empty");

        if (value.Length < MinLength || value.Length > MaxLength)
            throw new ArgumentException($"Content must be between {MinLength} and {MaxLength} characters");

        Value = value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
