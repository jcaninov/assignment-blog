namespace Blog.Application.IntegrationTests.Fixtures;

[CollectionDefinition(Name)]
public sealed class IntegrationTestCollection : ICollectionFixture<PostgreSqlContainerFixture>
{
    public const string Name = "Integration";
}
