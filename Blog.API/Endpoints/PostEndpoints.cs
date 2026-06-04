using Blog.Application.Abstractions;
using Blog.Application.Commands.CreatePost;
using Blog.Application.DTOs;
using Blog.Application.Queries.GetPostById;

namespace Blog.API.Endpoints;

public static class PostEndpoints
{
    public static void MapPostEndpoints(this WebApplication app)
    {
        app.MapPost("/posts", CreatePost)
            .WithName("CreatePost")
            .Produces<PostDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapGet("/posts/{id:guid}", GetPostById)
            .WithName("GetPostById")
            .Produces<PostDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> CreatePost(
        CreatePostRequest request,
        ICommandHandler<CreatePostCommand, PostDto> commandHandler,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreatePostCommand(request.AuthorId, request.Title, request.Content);
            var result = await commandHandler.HandleAsync(command, cancellationToken);
            return Results.Created($"/posts/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> GetPostById(
        Guid id,
        bool includeAuthor,
        IQueryHandler<GetPostByIdQuery, PostDto?> queryHandler,
        CancellationToken cancellationToken)
    {
        var query = new GetPostByIdQuery(id, includeAuthor);
        var result = await queryHandler.HandleAsync(query, cancellationToken);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}

public record CreatePostRequest(Guid AuthorId, string Title, string Content);
