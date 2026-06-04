using Blog.Domain.SharedKernel;

namespace Blog.Domain.Tests.SharedKernel;

public class ValueObjectEqualityTests
{
    private sealed class TestVoA : ValueObject
    {
        public string Value { get; }

        public TestVoA(string value) => Value = value;

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }
    }

    private sealed class TestVoB : ValueObject
    {
        public string Value { get; }

        public TestVoB(string value) => Value = value;

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }
    }

    [Fact]
    public void Equals_SameTypeAndValue_ReturnsTrue()
    {
        var left = new TestVoA("same");
        var right = new TestVoA("same");

        Assert.True(left.Equals(right));
        Assert.True(left == right);
    }

    [Fact]
    public void Equals_SameTypeDifferentValue_ReturnsFalse()
    {
        var left = new TestVoA("one");
        var right = new TestVoA("two");

        Assert.False(left.Equals(right));
        Assert.True(left != right);
    }

    [Fact]
    public void Equals_DifferentTypeSameValue_ObjectOverloadReturnsFalse()
    {
        var left = new TestVoA("same");
        var right = new TestVoB("same");

        Assert.False(left.Equals((object)right));
    }

    [Fact]
    public void GetHashCode_EqualInstances_ReturnSameHashCode()
    {
        var left = new TestVoA("same");
        var right = new TestVoA("same");

        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
