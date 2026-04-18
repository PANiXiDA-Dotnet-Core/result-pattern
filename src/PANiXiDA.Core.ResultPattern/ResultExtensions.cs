namespace PANiXiDA.Core.ResultPattern;

/// <summary>
/// Provides helper methods for composing and transforming <see cref="Result"/> and <see cref="Result{TValue}"/>.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Transforms a successful non-generic result into a successful generic result.
    /// </summary>
    /// <typeparam name="TOut">The type of the resulting value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="map">The mapping function to execute on success.</param>
    /// <returns>
    /// A successful <see cref="Result{TOut}"/> with the transformed value, or a failed result with the source result errors preserved.
    /// </returns>
    public static Result<TOut> Map<TOut>(this Result result, Func<TOut> map)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(map);

        if (result.IsFailure)
        {
            return Result.Failure<TOut>(result.Errors);
        }

        return Result.Success(map());
    }

    /// <summary>
    /// Transforms a successful generic result into another successful generic result.
    /// </summary>
    /// <typeparam name="TIn">The type of the source value.</typeparam>
    /// <typeparam name="TOut">The type of the resulting value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="map">The mapping function to execute on success.</param>
    /// <returns>
    /// A successful <see cref="Result{TOut}"/> with the transformed value, or a failed result with the source result errors preserved.
    /// </returns>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> map)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(map);

        if (result.IsFailure)
        {
            return Result.Failure<TOut>(result.Errors);
        }

        return Result.Success(map(result.Value));
    }

    /// <summary>
    /// Continues a successful generic result with a function that returns a non-generic result.
    /// </summary>
    /// <typeparam name="TIn">The type of the source value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="bind">The continuation function to execute on success.</param>
    /// <returns>
    /// The result returned by <paramref name="bind"/> if the source result is successful; otherwise, a failed result with the source result errors preserved.
    /// </returns>
    public static Result Bind<TIn>(this Result<TIn> result, Func<TIn, Result> bind)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(bind);

        if (result.IsFailure)
        {
            return Result.Failure(result.Errors);
        }

        return bind(result.Value);
    }

    /// <summary>
    /// Continues a successful generic result with a function that returns another generic result.
    /// </summary>
    /// <typeparam name="TIn">The type of the source value.</typeparam>
    /// <typeparam name="TOut">The type of the continuation result value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="bind">The continuation function to execute on success.</param>
    /// <returns>
    /// The result returned by <paramref name="bind"/> if the source result is successful; otherwise, a failed result with the source result errors preserved.
    /// </returns>
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> bind)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(bind);

        if (result.IsFailure)
        {
            return Result.Failure<TOut>(result.Errors);
        }

        return bind(result.Value);
    }

    /// <summary>
    /// Continues a successful non-generic result with a function that returns a generic result.
    /// </summary>
    /// <typeparam name="TOut">The type of the continuation result value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="bind">The continuation function to execute on success.</param>
    /// <returns>
    /// The result returned by <paramref name="bind"/> if the source result is successful; otherwise, a failed result with the source result errors preserved.
    /// </returns>
    public static Result<TOut> Bind<TOut>(this Result result, Func<Result<TOut>> bind)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(bind);

        if (result.IsFailure)
        {
            return Result.Failure<TOut>(result.Errors);
        }

        return bind();
    }

    /// <summary>
    /// Asynchronously continues a successful generic result with a function that returns a generic result.
    /// </summary>
    /// <typeparam name="TIn">The type of the source value.</typeparam>
    /// <typeparam name="TOut">The type of the continuation result value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="bind">The asynchronous continuation function to execute on success.</param>
    /// <returns>
    /// A task returning the result of <paramref name="bind"/> if the source result is successful; otherwise, a failed result with the source result errors preserved.
    /// </returns>
    public static Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> bind)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(bind);

        if (result.IsFailure)
        {
            return Task.FromResult(Result.Failure<TOut>(result.Errors));
        }

        return bind(result.Value);
    }

    /// <summary>
    /// Asynchronously continues a successful non-generic result with a function that returns a generic result.
    /// </summary>
    /// <typeparam name="TOut">The type of the continuation result value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="bind">The asynchronous continuation function to execute on success.</param>
    /// <returns>
    /// A task returning the result of <paramref name="bind"/> if the source result is successful; otherwise, a failed result with the source result errors preserved.
    /// </returns>
    public static Task<Result<TOut>> BindAsync<TOut>(this Result result, Func<Task<Result<TOut>>> bind)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(bind);

        if (result.IsFailure)
        {
            return Task.FromResult(Result.Failure<TOut>(result.Errors));
        }

        return bind();
    }

    /// <summary>
    /// Validates a successful generic result with an additional predicate.
    /// </summary>
    /// <typeparam name="TIn">The type of the source value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="predicate">The predicate that must return <see langword="true"/> for the result to remain successful.</param>
    /// <param name="error">The error returned when the predicate fails.</param>
    /// <returns>
    /// The source result if it is already failed or if the predicate succeeds; otherwise, a failed result with the <paramref name="error"/> error.
    /// </returns>
    public static Result<TIn> Ensure<TIn>(this Result<TIn> result, Func<TIn, bool> predicate, Error error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value)
            ? result
            : Result.Failure<TIn>(error);
    }

    /// <summary>
    /// Executes a side effect for a successful generic result and returns the original result.
    /// </summary>
    /// <typeparam name="TIn">The type of the source value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="action">The side effect to execute on success.</param>
    /// <returns>The source result.</returns>
    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsFailure)
        {
            return result;
        }

        action(result.Value);
        return result;
    }

    /// <summary>
    /// Converts a non-generic result into a final value by handling both success and failure.
    /// </summary>
    /// <typeparam name="TOut">The type of the final value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">The function to execute on success.</param>
    /// <param name="onFailure">The function to execute on failure.</param>
    /// <returns>The value returned by the matching branch.</returns>
    public static TOut Match<TOut>(this Result result, Func<TOut> onSuccess, Func<IReadOnlyList<Error>, TOut> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return result.IsSuccess
            ? onSuccess()
            : onFailure(result.Errors);
    }

    /// <summary>
    /// Converts a generic result into a final value by handling both success and failure.
    /// </summary>
    /// <typeparam name="TIn">The type of the source value.</typeparam>
    /// <typeparam name="TOut">The type of the final value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">The function to execute on success.</param>
    /// <param name="onFailure">The function to execute on failure.</param>
    /// <returns>The value returned by the matching branch.</returns>
    public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> onSuccess, Func<IReadOnlyList<Error>, TOut> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Errors);
    }
}
