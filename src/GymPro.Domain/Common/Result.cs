namespace GymPro.Domain.Common;

/// <summary>
/// The single error type the whole application speaks. Domain code never throws
/// for expected business failures (an expired membership, a duplicate national
/// code); it returns a <see cref="Result"/>, and only genuine programming
/// errors are exceptions.
/// </summary>
public sealed record DomainError(string Code, string Message, ErrorKind Kind = ErrorKind.Validation)
{
    public static readonly DomainError None = new(string.Empty, string.Empty);

    public static DomainError Validation(string field, string message) =>
        new($"validation.{field}", message, ErrorKind.Validation);

    public static DomainError NotFound(string what, object key) =>
        new($"notfound.{what}", $"{what} with key '{key}' was not found.", ErrorKind.NotFound);

    public static DomainError Conflict(string what, string message) =>
        new($"conflict.{what}", message, ErrorKind.Conflict);

    public static DomainError Forbidden(string message) =>
        new("forbidden", message, ErrorKind.Forbidden);

    public static DomainError Unexpected(string message) =>
        new("unexpected", message, ErrorKind.Unexpected);
}

public enum ErrorKind
{
    Validation = 0,
    NotFound = 1,
    Conflict = 2,
    Forbidden = 3,
    Unexpected = 4,
}

/// <summary>
/// Outcome of an operation that can fail for expected reasons.
/// </summary>
public class Result
{
    protected Result(bool isSuccess, DomainError error)
    {
        if (isSuccess && error != DomainError.None)
        {
            throw new InvalidOperationException("A successful result cannot carry an error.");
        }

        if (!isSuccess && error == DomainError.None)
        {
            throw new InvalidOperationException("A failed result must carry an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public DomainError Error { get; }

    public static Result Success() => new(true, DomainError.None);

    public static Result Failure(DomainError error) => new(false, error);

    public static Result<T> Success<T>(T value) => new(value, true, DomainError.None);

    public static Result<T> Failure<T>(DomainError error) => new(default, false, error);

    /// <summary>Collapses a failed result into the result's error, or yields <paramref name="value"/>.</summary>
    public static Result<T> From<T>(Result result, T value) =>
        result.IsSuccess ? Success(value) : Failure<T>(result.Error);
}

/// <summary>
/// Result carrying a value on success.
/// </summary>
public sealed class Result<T> : Result
{
    private readonly T? _value;

    internal Result(T? value, bool isSuccess, DomainError error)
        : base(isSuccess, error) => _value = value;

    /// <summary>
    /// The value. Accessing it on a failed result is a bug in the caller, not a
    /// business condition, so it throws loudly instead of returning default.
    /// </summary>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException($"Cannot read the value of a failed result: {Error.Code}.");

    public static implicit operator T(Result<T> result) => result.Value;

    public static implicit operator Result<T>(T value) => Success(value);
}
