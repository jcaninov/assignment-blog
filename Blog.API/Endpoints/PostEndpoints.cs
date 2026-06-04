using Blog.API.Contracts;
using Blog.API.Serialization;
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
            .Accepts<CreatePostRequest>(JsonApiContentSerializer.MediaType)
            .Produces<PostDto>(StatusCodes.Status201Created, contentType: JsonApiContentSerializer.MediaType)
            .Produces(StatusCodes.Status400BadRequest, contentType: JsonApiContentSerializer.MediaType)
            .Produces(StatusCodes.Status415UnsupportedMediaType, contentType: JsonApiContentSerializer.MediaType);

        app.MapGet("/posts/{id:guid}", GetPostById)
            .WithName("GetPostById")
            .Produces<PostDto>(StatusCodes.Status200OK, contentType: JsonApiContentSerializer.MediaType)
            .Produces(StatusCodes.Status404NotFound, contentType: JsonApiContentSerializer.MediaType)
            .Produces(StatusCodes.Status406NotAcceptable, contentType: JsonApiContentSerializer.MediaType);
    }

    private static async Task<IResult> CreatePost(
        HttpContext http,
        IApiContentSerializerResolver resolver,
        ICommandHandler<CreatePostCommand, PostDto> commandHandler,
        CancellationToken cancellationToken)
    {
        IApiContentSerializer requestSerializer;
        try
        {
            requestSerializer = resolver.ResolveForRequest(http.Request);
        }
        catch (UnsupportedMediaTypeException ex)
        {
            return ApiResults.UnsupportedMediaType(http, resolver, ex.MediaType);
        }

        var request = await requestSerializer.DeserializeAsync<CreatePostRequest>(http.Request.Body, cancellationToken);

        if (request is null)
            return ApiResults.BadRequest(http, resolver, new { error = "Invalid request body." });

        try
        {
            var command = new CreatePostCommand(request.AuthorId, request.Title, request.Description, request.Content);
            var result = await commandHandler.HandleAsync(command, cancellationToken);
            return ApiResults.Created(http, resolver, $"/posts/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            return ApiResults.BadRequest(http, resolver, new { error = ex.Message });
        }
        catch (NotAcceptableException ex)
        {
            return ApiResults.NotAcceptable(http, resolver, ex.AcceptHeader);
        }
    }

    private static async Task<IResult> GetPostById(
        HttpContext http,
        Guid id,
        bool includeAuthor,
        IApiContentSerializerResolver resolver,
        IQueryHandler<GetPostByIdQuery, PostDto?> queryHandler,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetPostByIdQuery(id, includeAuthor);
            var result = await queryHandler.HandleAsync(query, cancellationToken);
            return result is null
                ? ApiResults.NotFound(http, resolver)
                : ApiResults.Ok(http, resolver, result);
        }
        catch (NotAcceptableException ex)
        {
            return ApiResults.NotAcceptable(http, resolver, ex.AcceptHeader);
        }
    }
}
