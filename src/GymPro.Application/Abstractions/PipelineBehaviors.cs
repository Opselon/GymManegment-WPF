using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace GymPro.Application.Abstractions;

/// <summary>
/// Runs FluentValidation validators registered for the request type, before the
/// handler sees it. A failing validator aborts the request with a
/// <see cref="ValidationException"/>.
/// </summary>
public interface IRequestPipelineBehavior
{
    Task<TResponse> Handle<TResponse>(
        IRequest<TResponse> request,
        NextHandler<TResponse> continuation,
        CancellationToken cancellationToken);
}

/// <summary>Continuation of the pipeline.</summary>
public delegate Task<TResponse> NextHandler<TResponse>(CancellationToken cancellationToken);

/// <summary>Validates the request with any matching FluentValidation validator.</summary>
public sealed class ValidationPipelineBehavior(IEnumerable<IValidator> validators) : IRequestPipelineBehavior
{
    public async Task<TResponse> Handle<TResponse>(
        IRequest<TResponse> request,
        NextHandler<TResponse> continuation,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<object>(request);
        var failures = (await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken))))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count > 0)
            throw new ValidationException("Request validation failed.", failures.Select(f => f.ErrorMessage).Distinct().ToList());

        return await continuation(cancellationToken);
    }
}
