namespace PANiXiDA.Core.ResultPattern;

/// <summary>
/// Represents the result of an operation that can complete successfully with a value or fail with an error.
/// </summary>
/// <typeparam name="TValue">The type of the successful value.</typeparam>
public sealed class Result<TValue> : Result
{
    private readonly TValue? value;

    /// <summary>
    /// Initializes a new successful instance of <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="value">The successful value.</param>
    internal Result(TValue value)
        : base(true, EmptyErrors)
    {
        this.value = value;
    }

    /// <summary>
    /// Initializes a new failed instance of <see cref="Result{TValue}"/>.
    /// </summary>
    /// <param name="errors">Errors associated with the failed result.</param>
    internal Result(Error[] errors)
        : base(false, errors)
    {
        value = default;
    }

    /// <summary>
    /// Gets the successful result value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the result is a failure.</exception>
    public TValue Value
    {
        get
        {
            if (IsFailure)
            {
                throw new InvalidOperationException("Cannot access value of failed result.");
            }

            return value!;
        }
    }

    /// <summary>
    /// Gets the successful value or <see langword="default"/> if the result is a failure.
    /// </summary>
    public TValue? ValueOrDefault
    {
        get
        {
            return IsSuccess ? value : default;
        }
    }

    /// <summary>
    /// Attempts to get the successful value without throwing an exception.
    /// </summary>
    /// <param name="resultValue">
    /// When this method returns, contains the successful value or <see langword="default"/> if the result is a failure.
    /// </param>
    /// <returns><see langword="true"/> if the result is successful; otherwise, <see langword="false"/>.</returns>
    public bool TryGetValue(out TValue? resultValue)
    {
        if (IsSuccess)
        {
            resultValue = value;
            return true;
        }

        resultValue = default;
        return false;
    }
}
