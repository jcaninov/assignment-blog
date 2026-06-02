using Blog.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application;

public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> DispatchAsync<TResponse>(ICommand command, CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse));

        dynamic handler = _serviceProvider.GetRequiredService(handlerType);
        return await handler.HandleAsync(command, cancellationToken);
    }

    public async Task DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

        dynamic handler = _serviceProvider.GetRequiredService(handlerType);
        await handler.HandleAsync(command, cancellationToken);
    }
}
