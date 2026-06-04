using Blog.Application.Abstractions;
using Blog.Application.Commands.CreatePost;
using Blog.Application.DTOs;
using Blog.Application.IntegrationTests.Fixtures;
using Blog.Application.IntegrationTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application.IntegrationTests.Commands;

[Collection(IntegrationTestCollection.Name)]
[Trait("Category", "Integration")]
public sealed class CreatePostCommandHandlerTests
{
    private readonly PostgreSqlContainerFixture _fixture;

    public CreatePostCommandHandlerTests(PostgreSqlContainerFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task HandleAsync_WhenAuthorExists_PersistsPostAndDomainEvent()
    {
        var authorId = await TestDataHelper.InsertAuthorAsync(_fixture.ConnectionString);
        var command = new CreatePostCommand(authorId, "Integration Title", "Integration Description", "Integration Content");

        PostDto result;
        using (var scope = _fixture.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<CreatePostCommand, PostDto>>();
            result = await handler.HandleAsync(command);
        }

        Assert.Equal("Integration Title", result.Title);
        Assert.Equal("Integration Description", result.Description);
        Assert.Equal("Integration Content", result.Content);
        Assert.Equal(authorId, result.AuthorId);
        Assert.NotEqual(Guid.Empty, result.Id);

        Assert.True(await TestDataHelper.PostExistsAsync(_fixture.ConnectionString, result.Id));
        Assert.Equal(1, await TestDataHelper.CountDomainEventsForAggregateAsync(_fixture.ConnectionString, result.Id));
    }

    [Fact]
    public async Task HandleAsync_WhenAuthorMissing_ThrowsArgumentException()
    {
        var command = new CreatePostCommand(Guid.NewGuid(), "Title", "Description", "Content");

        using var scope = _fixture.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<CreatePostCommand, PostDto>>();

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(command));

        Assert.Contains("was not found", ex.Message);
    }
}
