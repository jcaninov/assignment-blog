namespace Blog.Application.Abstractions;

public interface ICommand
{
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand
{
    Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
