using System.Data;
using Blog.Application.Abstractions;
using Blog.Application.Commands.CreatePost;
using Blog.Application.DTOs;
using Blog.Application.Queries.GetPostById;
using Blog.Domain.Repositories;
using Blog.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Blog.Application.IntegrationTests.Infrastructure;

internal static class ApplicationServiceProviderFactory
{
    public static IServiceScope CreateScope(string connectionString)
    {
        var services = new ServiceCollection();

        services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(connectionString));
        services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IDomainEventRepository, DomainEventRepository>()
            .AddScoped<IPostRepository, PostRepositoryDapper>()
            .AddScoped<IAuthorRepository, AuthorRepositoryDapper>();

        services
            .AddScoped<ICommandHandler<CreatePostCommand, PostDto>, CreatePostCommandHandler>()
            .AddScoped<IQueryHandler<GetPostByIdQuery, PostDto?>, GetPostByIdQueryHandler>();

        return services.BuildServiceProvider().CreateScope();
    }
}
