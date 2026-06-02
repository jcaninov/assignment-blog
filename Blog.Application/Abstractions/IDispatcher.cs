namespace Blog.Application.Abstractions;

public interface ICommandDispatcher
{
    Task<TResponse> DispatchAsync<TResponse>(ICommand command, CancellationToken cancellationToken = default);
    Task DispatchAsync(ICommand command, CancellationToken cancellationToken = default);
}

public interface IQueryDispatcher
{
    Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
}
