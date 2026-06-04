using Blog.Application.Abstractions;
using Blog.Application.Commands.CreatePost;
using Blog.Application.DTOs;
using Blog.Application.IntegrationTests.Fixtures;
using Blog.Application.IntegrationTests.Infrastructure;
using Blog.Application.Queries.GetPostById;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application.IntegrationTests.Queries;

[Collection(IntegrationTestCollection.Name)]
[Trait("Category", "Integration")]
public sealed class GetPostByIdQueryHandlerTests
{
    private readonly PostgreSqlContainerFixture _fixture;

    public GetPostByIdQueryHandlerTests(PostgreSqlContainerFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task HandleAsync_WhenPostExists_ReturnsPostDto()
    {
        var authorId = await TestDataHelper.InsertAuthorAsync(_fixture.ConnectionString);
        var createCommand = new CreatePostCommand(authorId, "Query Title", "Query Description", "Query Content");

        PostDto created;
        using (var createScope = _fixture.CreateScope())
        {
            var createHandler = createScope.ServiceProvider.GetRequiredService<ICommandHandler<CreatePostCommand, PostDto>>();
            created = await createHandler.HandleAsync(createCommand);
        }

        using var queryScope = _fixture.CreateScope();
        var queryHandler = queryScope.ServiceProvider.GetRequiredService<IQueryHandler<GetPostByIdQuery, PostDto?>>();
        var result = await queryHandler.HandleAsync(new GetPostByIdQuery(created.Id, includeAuthor: false));

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("Query Title", result.Title);
        Assert.Equal(authorId, result.AuthorId);
        Assert.Null(result.Author);
    }

    [Fact]
    public async Task HandleAsync_WhenPostMissing_ReturnsNull()
    {
        using var scope = _fixture.CreateScope();
        var queryHandler = scope.ServiceProvider.GetRequiredService<IQueryHandler<GetPostByIdQuery, PostDto?>>();

        var result = await queryHandler.HandleAsync(new GetPostByIdQuery(Guid.NewGuid(), includeAuthor: false));

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WithIncludeAuthor_ReturnsAuthorInDto()
    {
        var authorId = await TestDataHelper.InsertAuthorAsync(_fixture.ConnectionString, "Alice", "Smith");
        var createCommand = new CreatePostCommand(authorId, "Title", "Description", "Content");

        PostDto created;
        using (var createScope = _fixture.CreateScope())
        {
            var createHandler = createScope.ServiceProvider.GetRequiredService<ICommandHandler<CreatePostCommand, PostDto>>();
            created = await createHandler.HandleAsync(createCommand);
        }

        using var queryScope = _fixture.CreateScope();
        var queryHandler = queryScope.ServiceProvider.GetRequiredService<IQueryHandler<GetPostByIdQuery, PostDto?>>();
        var result = await queryHandler.HandleAsync(new GetPostByIdQuery(created.Id, includeAuthor: true));

        Assert.NotNull(result);
        Assert.NotNull(result.Author);
        Assert.Equal(authorId, result.Author.Id);
        Assert.Equal("Alice", result.Author.Name);
        Assert.Equal("Smith", result.Author.Surname);
    }
}
