using System.Collections.ObjectModel;

namespace PANiXiDA.Core.ResultPattern;

/// <summary>
/// Represents the result of an operation that can complete successfully or fail with an error.
/// </summary>
public class Result
{
    /// <summary>
    /// An empty error collection used for successful results.
    /// </summary>
    protected static readonly Error[] EmptyErrors = [];

    /// <summary>
    /// Initializes a new instance of <see cref="Result"/>.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the result is successful.</param>
    /// <param name="errors">Errors associated with the result.</param>
    protected Result(bool isSuccess, Error[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        if (isSuccess && errors.Length > 0)
        {
            throw new ArgumentException("Successful result cannot contain errors.", nameof(errors));
        }

        if (!isSuccess && errors.Length == 0)
        {
            throw new ArgumentException("Failed result must contain at least one error.", nameof(errors));
        }

        IsSuccess = isSuccess;
        Errors = new ReadOnlyCollection<Error>(errors.ToArray());
    }

    /// <summary>
    /// Gets a value indicating whether the result is successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result completed with an error.
    /// </summary>
    public bool IsFailure
    {
        get
        {
            return !IsSuccess;
        }
    }

    /// <summary>
    /// Gets the collection of errors associated with the result.
    /// </summary>
    public IReadOnlyList<Error> Errors { get; }

    /// <summary>
    /// Gets the first error of the result.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is successful.</exception>
    public Error FirstError
    {
        get
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException("Successful result does not contain errors.");
            }

            return Errors[0];
        }
    }

    /// <summary>
    /// Creates a successful result without a value.
    /// </summary>
    /// <returns>A successful instance of <see cref="Result"/>.</returns>
    public static Result Success()
    {
        return new Result(true, EmptyErrors);
    }

    /// <summary>
    /// Creates a successful result with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the successful value.</typeparam>
    /// <param name="value">The value to wrap in the result.</param>
    /// <returns>A successful instance of <see cref="Result{TValue}"/>.</returns>
    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(value);
    }

    /// <summary>
    /// Creates a failed result with a single error.
    /// </summary>
    /// <param name="error">The error associated with the failed result.</param>
    /// <returns>A failed instance of <see cref="Result"/>.</returns>
    public static Result Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new Result(false, [error]);
    }

    /// <summary>
    /// Creates a failed result with multiple errors.
    /// </summary>
    /// <param name="errors">Errors associated with the failed result.</param>
    /// <returns>A failed instance of <see cref="Result"/>.</returns>
    public static Result Failure(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var materializedErrors = errors.Where(item => item is not null).ToArray();

        if (materializedErrors.Length == 0)
        {
            throw new ArgumentException("Failed result must contain at least one error.", nameof(errors));
        }

        return new Result(false, materializedErrors);
    }

    /// <summary>
    /// Creates a failed generic result with a single error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would be returned on success.</typeparam>
    /// <param name="error">The error associated with the failed result.</param>
    /// <returns>A failed instance of <see cref="Result{TValue}"/>.</returns>
    public static Result<TValue> Failure<TValue>(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new Result<TValue>([error]);
    }

    /// <summary>
    /// Creates a failed generic result with multiple errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the value that would be returned on success.</typeparam>
    /// <param name="errors">Errors associated with the failed result.</param>
    /// <returns>A failed instance of <see cref="Result{TValue}"/>.</returns>
    public static Result<TValue> Failure<TValue>(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var materializedErrors = errors.Where(static item => item is not null).ToArray();

        if (materializedErrors.Length == 0)
        {
            throw new ArgumentException("Failed result must contain at least one error.", nameof(errors));
        }

        return new Result<TValue>(materializedErrors);
    }

    /// <summary>
    /// Combines multiple results into a single result.
    /// </summary>
    /// <param name="results">The results to combine.</param>
    /// <returns>
    /// A successful result if all provided results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result Combine(params Result[] results)
    {
        ArgumentNullException.ThrowIfNull(results);

        if (results.Any(static item => item is null))
        {
            throw new ArgumentException("Results collection cannot contain null values.", nameof(results));
        }

        var errors = results
            .Where(item => item.IsFailure)
            .SelectMany(item => item.Errors)
            .ToArray();

        if (errors.Length == 0)
        {
            return Success();
        }

        return Failure(errors);
    }
}
