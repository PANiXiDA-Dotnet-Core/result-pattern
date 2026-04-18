namespace PANiXiDA.Core.ResultPattern;

/// <summary>
/// Provides helper methods for combining multiple generic results.
/// </summary>
public static class ResultCombiner
{
    /// <summary>
    /// Combines two generic results into a single tuple result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <returns>
    /// A successful result containing a tuple of values if both results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<(T1, T2)> Combine<T1, T2>(
        Result<T1> first,
        Result<T2> second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        return CombineCore(
            () => Result.Combine(first, second),
            () => Result.Success((first.Value, second.Value)));
    }

    /// <summary>
    /// Combines two generic results and continues the successful result with a function that returns another generic result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="TOut">The type of the continuation result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="bind">The continuation function to execute when all results are successful.</param>
    /// <returns>
    /// The result returned by <paramref name="bind"/> if both results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<TOut> Combine<T1, T2, TOut>(
        Result<T1> first,
        Result<T2> second,
        Func<T1, T2, Result<TOut>> bind)
    {
        ArgumentNullException.ThrowIfNull(bind);

        return Combine(first, second)
            .Bind(values => bind(values.Item1, values.Item2));
    }

    /// <summary>
    /// Combines three generic results into a single tuple result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="T3">The type of the third result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <returns>
    /// A successful result containing a tuple of values if all results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<(T1, T2, T3)> Combine<T1, T2, T3>(
        Result<T1> first,
        Result<T2> second,
        Result<T3> third)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(third);

        return CombineCore(
            () => Result.Combine(first, second, third),
            () => Result.Success((first.Value, second.Value, third.Value)));
    }

    /// <summary>
    /// Combines three generic results and continues the successful result with a function that returns another generic result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="T3">The type of the third result value.</typeparam>
    /// <typeparam name="TOut">The type of the continuation result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="bind">The continuation function to execute when all results are successful.</param>
    /// <returns>
    /// The result returned by <paramref name="bind"/> if all results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<TOut> Combine<T1, T2, T3, TOut>(
        Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Func<T1, T2, T3, Result<TOut>> bind)
    {
        ArgumentNullException.ThrowIfNull(bind);

        return Combine(first, second, third)
            .Bind(values => bind(values.Item1, values.Item2, values.Item3));
    }

    /// <summary>
    /// Combines four generic results into a single tuple result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="T3">The type of the third result value.</typeparam>
    /// <typeparam name="T4">The type of the fourth result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <returns>
    /// A successful result containing a tuple of values if all results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<(T1, T2, T3, T4)> Combine<T1, T2, T3, T4>(
        Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(fourth);

        return CombineCore(
            () => Result.Combine(first, second, third, fourth),
            () => Result.Success((first.Value, second.Value, third.Value, fourth.Value)));
    }

    /// <summary>
    /// Combines four generic results and continues the successful result with a function that returns another generic result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="T3">The type of the third result value.</typeparam>
    /// <typeparam name="T4">The type of the fourth result value.</typeparam>
    /// <typeparam name="TOut">The type of the continuation result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <param name="bind">The continuation function to execute when all results are successful.</param>
    /// <returns>
    /// The result returned by <paramref name="bind"/> if all results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<TOut> Combine<T1, T2, T3, T4, TOut>(
        Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Func<T1, T2, T3, T4, Result<TOut>> bind)
    {
        ArgumentNullException.ThrowIfNull(bind);

        return Combine(first, second, third, fourth)
            .Bind(values => bind(values.Item1, values.Item2, values.Item3, values.Item4));
    }

    /// <summary>
    /// Combines five generic results into a single tuple result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="T3">The type of the third result value.</typeparam>
    /// <typeparam name="T4">The type of the fourth result value.</typeparam>
    /// <typeparam name="T5">The type of the fifth result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <param name="fifth">The fifth result.</param>
    /// <returns>
    /// A successful result containing a tuple of values if all results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<(T1, T2, T3, T4, T5)> Combine<T1, T2, T3, T4, T5>(
        Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(fifth);

        return CombineCore(
            () => Result.Combine(first, second, third, fourth, fifth),
            () => Result.Success((first.Value, second.Value, third.Value, fourth.Value, fifth.Value)));
    }

    /// <summary>
    /// Combines five generic results and continues the successful result with a function that returns another generic result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="T3">The type of the third result value.</typeparam>
    /// <typeparam name="T4">The type of the fourth result value.</typeparam>
    /// <typeparam name="T5">The type of the fifth result value.</typeparam>
    /// <typeparam name="TOut">The type of the continuation result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <param name="fifth">The fifth result.</param>
    /// <param name="bind">The continuation function to execute when all results are successful.</param>
    /// <returns>
    /// The result returned by <paramref name="bind"/> if all results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<TOut> Combine<T1, T2, T3, T4, T5, TOut>(
        Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Func<T1, T2, T3, T4, T5, Result<TOut>> bind)
    {
        ArgumentNullException.ThrowIfNull(bind);

        return Combine(first, second, third, fourth, fifth)
            .Bind(values => bind(values.Item1, values.Item2, values.Item3, values.Item4, values.Item5));
    }

    /// <summary>
    /// Combines six generic results into a single tuple result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="T3">The type of the third result value.</typeparam>
    /// <typeparam name="T4">The type of the fourth result value.</typeparam>
    /// <typeparam name="T5">The type of the fifth result value.</typeparam>
    /// <typeparam name="T6">The type of the sixth result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <param name="fifth">The fifth result.</param>
    /// <param name="sixth">The sixth result.</param>
    /// <returns>
    /// A successful result containing a tuple of values if all results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<(T1, T2, T3, T4, T5, T6)> Combine<T1, T2, T3, T4, T5, T6>(
        Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Result<T6> sixth)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(sixth);

        return CombineCore(
            () => Result.Combine(first, second, third, fourth, fifth, sixth),
            () => Result.Success((first.Value, second.Value, third.Value, fourth.Value, fifth.Value, sixth.Value)));
    }

    /// <summary>
    /// Combines six generic results and continues the successful result with a function that returns another generic result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="T3">The type of the third result value.</typeparam>
    /// <typeparam name="T4">The type of the fourth result value.</typeparam>
    /// <typeparam name="T5">The type of the fifth result value.</typeparam>
    /// <typeparam name="T6">The type of the sixth result value.</typeparam>
    /// <typeparam name="TOut">The type of the continuation result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <param name="fifth">The fifth result.</param>
    /// <param name="sixth">The sixth result.</param>
    /// <param name="bind">The continuation function to execute when all results are successful.</param>
    /// <returns>
    /// The result returned by <paramref name="bind"/> if all results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<TOut> Combine<T1, T2, T3, T4, T5, T6, TOut>(
        Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Result<T6> sixth,
        Func<T1, T2, T3, T4, T5, T6, Result<TOut>> bind)
    {
        ArgumentNullException.ThrowIfNull(bind);

        return Combine(first, second, third, fourth, fifth, sixth)
            .Bind(values => bind(values.Item1, values.Item2, values.Item3, values.Item4, values.Item5, values.Item6));
    }

    /// <summary>
    /// Combines seven generic results into a single tuple result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="T3">The type of the third result value.</typeparam>
    /// <typeparam name="T4">The type of the fourth result value.</typeparam>
    /// <typeparam name="T5">The type of the fifth result value.</typeparam>
    /// <typeparam name="T6">The type of the sixth result value.</typeparam>
    /// <typeparam name="T7">The type of the seventh result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <param name="fifth">The fifth result.</param>
    /// <param name="sixth">The sixth result.</param>
    /// <param name="seventh">The seventh result.</param>
    /// <returns>
    /// A successful result containing a tuple of values if all results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<(T1, T2, T3, T4, T5, T6, T7)> Combine<T1, T2, T3, T4, T5, T6, T7>(
        Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Result<T6> sixth,
        Result<T7> seventh)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(third);
        ArgumentNullException.ThrowIfNull(fourth);
        ArgumentNullException.ThrowIfNull(fifth);
        ArgumentNullException.ThrowIfNull(sixth);
        ArgumentNullException.ThrowIfNull(seventh);

        return CombineCore(
            () => Result.Combine(first, second, third, fourth, fifth, sixth, seventh),
            () => Result.Success((first.Value, second.Value, third.Value, fourth.Value, fifth.Value, sixth.Value, seventh.Value)));
    }

    /// <summary>
    /// Combines seven generic results and continues the successful result with a function that returns another generic result.
    /// </summary>
    /// <typeparam name="T1">The type of the first result value.</typeparam>
    /// <typeparam name="T2">The type of the second result value.</typeparam>
    /// <typeparam name="T3">The type of the third result value.</typeparam>
    /// <typeparam name="T4">The type of the fourth result value.</typeparam>
    /// <typeparam name="T5">The type of the fifth result value.</typeparam>
    /// <typeparam name="T6">The type of the sixth result value.</typeparam>
    /// <typeparam name="T7">The type of the seventh result value.</typeparam>
    /// <typeparam name="TOut">The type of the continuation result value.</typeparam>
    /// <param name="first">The first result.</param>
    /// <param name="second">The second result.</param>
    /// <param name="third">The third result.</param>
    /// <param name="fourth">The fourth result.</param>
    /// <param name="fifth">The fifth result.</param>
    /// <param name="sixth">The sixth result.</param>
    /// <param name="seventh">The seventh result.</param>
    /// <param name="bind">The continuation function to execute when all results are successful.</param>
    /// <returns>
    /// The result returned by <paramref name="bind"/> if all results are successful; otherwise, a failed result with all collected errors.
    /// </returns>
    public static Result<TOut> Combine<T1, T2, T3, T4, T5, T6, T7, TOut>(
        Result<T1> first,
        Result<T2> second,
        Result<T3> third,
        Result<T4> fourth,
        Result<T5> fifth,
        Result<T6> sixth,
        Result<T7> seventh,
        Func<T1, T2, T3, T4, T5, T6, T7, Result<TOut>> bind)
    {
        ArgumentNullException.ThrowIfNull(bind);

        return Combine(first, second, third, fourth, fifth, sixth, seventh)
            .Bind(values => bind(values.Item1, values.Item2, values.Item3, values.Item4, values.Item5, values.Item6, values.Item7));
    }

    private static Result<TOut> CombineCore<TOut>(Func<Result> combine, Func<Result<TOut>> bind)
    {
        var combinedResult = combine();
        if (combinedResult.IsFailure)
        {
            return Result.Failure<TOut>(combinedResult.Errors);
        }

        return bind();
    }
}
