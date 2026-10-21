using GymPro.Domain.Common;

namespace GymPro.Application.Abstractions;

/// <summary>
/// Marker for request types handled by the in-process CQRS pipeline.
/// </summary>
/// <typeparam name="TResponse">Type of the response.</typeparam>
public interface IRequest<TResponse>;

/// <summary>
/// <summary>A request that performs a mutation. Command handlers run inside the unit of
/// work and are the only place aggregate roots are modified.</summary>
public interface ICommand<TResponse> : IRequest<TResponse>;

/// <summary>A request that reads data. Query handlers never modify aggregates.</summary>
public interface IQuery<TResponse> : IRequest<TResponse>;

/// <summary>Handles a request of the given type.</summary>
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>Handles a command of the given type.</summary>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>;

/// <summary>Handles a query of the given type.</summary>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>;

/// <summary>
/// The exception thrown when a command fails validation. Distinct from a domain
/// error so the pipeline can tell a pre-condition failure from an unexpected
/// fault.
/// </summary>
public sealed class ValidationException(string message, IReadOnlyList<string> errors)
    : Exception(message)
{
    public IReadOnlyList<string> Errors { get; } = errors;
}
