using Microsoft.Extensions.DependencyInjection;

namespace GymPro.Application.Abstractions;

/// <summary>
/// Resolves and invokes the handler for a request. Handlers are registered by
/// the closed generic type of the request, which the DI container resolves
/// directly — so this is the entire CQRS machinery, with no reflection-based
/// assembly scanning and no external mediation library.
/// </summary>
public interface IRequestDispatcher
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}

public sealed class RequestDispatcher(IServiceProvider services) : IRequestDispatcher
{
    public async Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        // Resolves IRequestHandler<TRequest, TResponse> for the closed type of
        // the concrete request, then invokes it through a bridge cast so the
        // compiler is satisfied without knowing TRequest statically.
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = services.GetRequiredService(handlerType);
        var bridgeType = typeof(HandlerBridge<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var bridge = (IHandlerBridge<TResponse>)Activator.CreateInstance(bridgeType, handler)!;
        return await bridge.Handle(request, cancellationToken);
    }
}

/// <summary>Invokes a handler without the request type being statically known.</summary>
public interface IHandlerBridge<TResponse>
{
    Task<TResponse> Handle(object request, CancellationToken cancellationToken);
}

/// <summary>Strongly-typed adapter from the boxed request to the real handler.</summary>
public sealed class HandlerBridge<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
    : IHandlerBridge<TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(object request, CancellationToken cancellationToken)
        => await handler.Handle((TRequest)request, cancellationToken);
}
