namespace PANiXiDA.Core.ResultPattern.UnitTests;

public sealed class GenericResultTests
{
    [Fact(DisplayName = "Success<T> → returns a successful generic result when a value is provided")]
    public void Success_Should_ReturnSuccessfulGenericResult_When_ValueIsProvided()
    {
        const int value = 42;

        var result = Result.Success(value);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
        result.Value.Should().Be(value);
    }

    [Fact(DisplayName = "Success<T> → returns a successful generic result when null is provided")]
    public void Success_Should_ReturnSuccessfulGenericResult_When_ValueIsNull()
    {
        string? value = null;

        var result = Result.Success(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
        result.ValueOrDefault.Should().BeNull();
    }

    [Fact(DisplayName = "Value → returns the value when the result is successful")]
    public void Value_Should_ReturnValue_When_ResultIsSuccess()
    {
        const string value = "value";

        var result = Result.Success(value);

        result.Value.Should().Be(value);
    }

    [Fact(DisplayName = "Value → throws InvalidOperationException when the result is a failure")]
    public void Value_Should_ThrowInvalidOperationException_When_ResultIsFailure()
    {
        var result = Result.Failure<string>(Error.Validation("Validation failed"));
        Action act = () => _ = result.Value;

        var exception = act.Should().Throw<InvalidOperationException>().Which;

        exception.Message.Should().Be("Cannot access value of failed result.");
    }

    [Fact(DisplayName = "ValueOrDefault → returns the value when the result is successful")]
    public void ValueOrDefault_Should_ReturnValue_When_ResultIsSuccess()
    {
        const string value = "value";

        var result = Result.Success(value);

        result.ValueOrDefault.Should().Be(value);
    }

    [Fact(DisplayName = "ValueOrDefault → returns default when the result is a failure")]
    public void ValueOrDefault_Should_ReturnDefault_When_ResultIsFailure()
    {
        var result = Result.Failure<string>(Error.Validation("Validation failed"));

        var value = result.ValueOrDefault;

        value.Should().BeNull();
    }

    [Fact(DisplayName = "TryGetValue → returns true and the value when the result is successful")]
    public void TryGetValue_Should_ReturnTrueAndValue_When_ResultIsSuccess()
    {
        const string value = "value";
        var result = Result.Success(value);

        var success = result.TryGetValue(out var actualValue);

        success.Should().BeTrue();
        actualValue.Should().Be(value);
    }

    [Fact(DisplayName = "TryGetValue → returns false and default when the result is a failure")]
    public void TryGetValue_Should_ReturnFalseAndDefault_When_ResultIsFailure()
    {
        var result = Result.Failure<string>(Error.Validation("Validation failed"));

        var success = result.TryGetValue(out var value);

        success.Should().BeFalse();
        value.Should().BeNull();
    }

    [Fact(DisplayName = "Failure<T>(Error) → returns a failed generic result when an error is provided")]
    public void Failure_Should_ReturnFailureGenericResult_When_ErrorIsProvided()
    {
        var error = Error.Validation("Validation failed");

        var result = Result.Failure<string>(error);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().BeSameAs(error);
    }

    [Fact(DisplayName = "Failure<T>(Error) → throws ArgumentNullException when error is null")]
    public void Failure_Should_ThrowArgumentNullException_When_ErrorIsNull()
    {
        Action act = () => Result.Failure<string>((Error)null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("error");
    }

    [Fact(DisplayName = "Failure<T>(IEnumerable<Error>) → returns a failed generic result when the collection contains errors")]
    public void Failure_Should_ReturnFailureGenericResult_When_ErrorCollectionContainsErrors()
    {
        var firstError = Error.Validation("Validation failed");
        var secondError = Error.NotFound("Not found");

        var result = Result.Failure<string>([firstError, secondError]);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Equal(firstError, secondError);
    }

    [Fact(DisplayName = "Failure<T>(IEnumerable<Error>) → filters out null values when the collection contains null")]
    public void Failure_Should_FilterOutNullErrors_When_ErrorCollectionContainsNullValues()
    {
        var error = Error.Validation("Validation failed");

        var result = Result.Failure<string>([error, null!]);

        result.Errors.Should().ContainSingle().Which.Should().BeSameAs(error);
    }

    [Fact(DisplayName = "Failure<T>(IEnumerable<Error>) → throws ArgumentNullException when the error collection is null")]
    public void Failure_Should_ThrowArgumentNullException_When_ErrorCollectionIsNull()
    {
        Action act = () => Result.Failure<string>((IEnumerable<Error>)null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("errors");
    }

    [Fact(DisplayName = "Failure<T>(IEnumerable<Error>) → throws ArgumentException when the error collection is empty")]
    public void Failure_Should_ThrowArgumentException_When_ErrorCollectionIsEmpty()
    {
        Action act = () => Result.Failure<string>([]);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("errors");
    }

    [Fact(DisplayName = "Failure<T>(IEnumerable<Error>) → throws ArgumentException when the collection contains only null")]
    public void Failure_Should_ThrowArgumentException_When_ErrorCollectionContainsOnlyNullValues()
    {
        Action act = () => Result.Failure<string>([null!]);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("errors");
    }
}
