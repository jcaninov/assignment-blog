using Blog.API;
using Blog.Application;
using Blog.Application.Abstractions;
using Blog.Application.Commands.CreatePost;
using Blog.Application.DTOs;
using Blog.Application.Queries.GetAllPosts;
using Blog.Domain.Repositories;
using Blog.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

// Register Domain & Infrastructure
// Register Domain & Infrastructure
builder.Services.AddTransient<System.Data.IDbConnection>(_ =>
    new Npgsql.NpgsqlConnection(builder.Configuration.GetConnectionString("Default") ?? Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? string.Empty));

builder.Services.AddScoped<Blog.Domain.Repositories.IPostRepository, Blog.Infrastructure.Persistence.PostRepositoryDapper>();
builder.Services.AddScoped<Blog.Domain.Repositories.IAuthorRepository, Blog.Infrastructure.Persistence.AuthorRepositoryDapper>();

// Register Application CQRS
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

// Register Command & Query Handlers
builder.Services.AddScoped<ICommandHandler<CreatePostCommand, PostDto>, CreatePostCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllPostsQuery, IReadOnlyList<PostDto>>, GetAllPostsQueryHandler>();

// API & Swagger
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.MapPostEndpoints();

app.Run();
