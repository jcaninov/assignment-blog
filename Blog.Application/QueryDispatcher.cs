using Blog.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application;

public sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var queryType = query.GetType();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));

        dynamic handler = _serviceProvider.GetRequiredService(handlerType);
        return await handler.HandleAsync(query, cancellationToken);
    }
}
