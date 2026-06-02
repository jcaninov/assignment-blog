using Blog.Application.Abstractions;
using Blog.Application.Commands.CreatePost;
using Blog.Application.DTOs;
using Blog.Application.Queries.GetAllPosts;

namespace Blog.API;

public static class PostEndpoints
{
    public static void MapPostEndpoints(this WebApplication app)
    {
        app.MapPost("/posts", CreatePost)
            .WithName("CreatePost")
            .Produces<PostDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapGet("/posts", GetAllPosts)
            .WithName("GetAllPosts")
            .Produces<IReadOnlyList<PostDto>>(StatusCodes.Status200OK);
    }

    private static async Task<IResult> CreatePost(
        CreatePostRequest request,
        ICommandDispatcher commandDispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreatePostCommand(request.Title, request.Content);
            var result = await commandDispatcher.DispatchAsync<PostDto>(command, cancellationToken);
            return Results.Created($"/posts/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    private static async Task<IResult> GetAllPosts(
        IQueryDispatcher queryDispatcher,
        CancellationToken cancellationToken)
    {
        var query = new GetAllPostsQuery();
        var result = await queryDispatcher.DispatchAsync(query, cancellationToken);
        return Results.Ok(result);
    }
}

public record CreatePostRequest(string Title, string Content);
