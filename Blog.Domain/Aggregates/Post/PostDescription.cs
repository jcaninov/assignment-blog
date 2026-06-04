using Blog.Domain.SharedKernel;

namespace Blog.Domain.Aggregates.Post;

public sealed class PostDescription : ValueObject
{
    public const int MaxLength = 1000;
    public const int MinLength = 1;

    public string Value { get; }

    public PostDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Description cannot be empty");

        if (value.Length < MinLength || value.Length > MaxLength)
            throw new ArgumentException($"Description must be between {MinLength} and {MaxLength} characters");

        Value = value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
