using Blog.API.Endpoints;
using Blog.API.Serialization;
using Blog.Application.Abstractions;
using Blog.Application.Commands.CreatePost;
using Blog.Application.DTOs;
using Blog.Application.Queries.GetPostById;
using Blog.Domain.Repositories;
using Blog.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.AddBlogApiSerialization(builder.Configuration);

// Register Infrastructure
builder.Services.AddTransient<System.Data.IDbConnection>(_ =>
    new Npgsql.NpgsqlConnection(builder.Configuration.GetConnectionString("Default") ?? Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? string.Empty));

builder.Services
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<IDomainEventRepository, DomainEventRepository>()
    .AddScoped<IPostRepository, PostRepositoryDapper>()
    .AddScoped<IAuthorRepository, AuthorRepositoryDapper>();

// Register Command & Query Handlers
builder.Services
    .AddScoped<ICommandHandler<CreatePostCommand, PostDto>, CreatePostCommandHandler>()
    .AddScoped<IQueryHandler<GetPostByIdQuery, PostDto?>, GetPostByIdQueryHandler>();

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

app.UseCors("AllowAll");

app.MapPostEndpoints();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
