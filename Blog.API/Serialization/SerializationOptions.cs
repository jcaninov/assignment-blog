namespace Blog.API.Serialization;

public sealed class SerializationOptions
{
    public const string SectionName = "Serialization";

    public ApiContentFormat DefaultFormat { get; set; } = ApiContentFormat.Json;
}
