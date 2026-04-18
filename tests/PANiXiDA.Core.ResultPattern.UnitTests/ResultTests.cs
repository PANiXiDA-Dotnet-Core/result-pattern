using System.Reflection;

namespace PANiXiDA.Core.ResultPattern.UnitTests;

public sealed class ResultTests
{
    [Fact(DisplayName = "Success() → returns a successful result when called without errors")]
    public void Success_Should_ReturnSuccessfulResult_When_Called()
    {
        var result = Result.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "FirstError → throws InvalidOperationException when the result is successful")]
    public void FirstError_Should_ThrowInvalidOperationException_When_ResultIsSuccess()
    {
        var result = Result.Success();
        Action act = () => _ = result.FirstError;

        var exception = act.Should().Throw<InvalidOperationException>().Which;

        exception.Message.Should().Be("Successful result does not contain errors.");
    }

    [Fact(DisplayName = "FirstError → returns the first error when the result is a failure")]
    public void FirstError_Should_ReturnFirstError_When_ResultIsFailure()
    {
        var firstError = Error.Validation("Validation failed");
        var secondError = Error.Conflict("Conflict");
        var result = Result.Failure([firstError, secondError]);

        var firstResultError = result.FirstError;

        firstResultError.Should().BeSameAs(firstError);
    }

    [Fact(DisplayName = "Failure(Error) → returns a failed result when an error is provided")]
    public void Failure_Should_ReturnFailureResult_When_ErrorIsProvided()
    {
        var error = Error.Validation("Validation failed");

        var result = Result.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().BeSameAs(error);
    }

    [Fact(DisplayName = "Failure(Error) → throws ArgumentNullException when error is null")]
    public void Failure_Should_ThrowArgumentNullException_When_ErrorIsNull()
    {
        Action act = () => Result.Failure((Error)null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("error");
    }

    [Fact(DisplayName = "Failure(IEnumerable<Error>) → returns a failed result when the collection contains errors")]
    public void Failure_Should_ReturnFailureResult_When_ErrorCollectionContainsErrors()
    {
        var firstError = Error.Validation("Validation failed");
        var secondError = Error.Conflict("Conflict");

        var result = Result.Failure([firstError, secondError]);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Equal(firstError, secondError);
    }

    [Fact(DisplayName = "Failure(IEnumerable<Error>) → filters out null values when the collection contains null")]
    public void Failure_Should_FilterOutNullErrors_When_ErrorCollectionContainsNullValues()
    {
        var error = Error.Validation("Validation failed");

        var result = Result.Failure([error, null!]);

        result.Errors.Should().ContainSingle().Which.Should().BeSameAs(error);
    }

    [Fact(DisplayName = "Failure(IEnumerable<Error>) → preserves error order when the collection contains multiple errors")]
    public void Failure_Should_PreserveErrorOrder_When_ErrorCollectionContainsMultipleErrors()
    {
        var firstError = Error.Validation("Validation failed");
        var secondError = Error.NotFound("Not found");
        var thirdError = Error.Conflict("Conflict");

        var result = Result.Failure([firstError, secondError, thirdError]);

        result.Errors.Should().Equal(firstError, secondError, thirdError);
    }

    [Fact(DisplayName = "Failure(IEnumerable<Error>) → throws ArgumentNullException when the error collection is null")]
    public void Failure_Should_ThrowArgumentNullException_When_ErrorCollectionIsNull()
    {
        Action act = () => Result.Failure((IEnumerable<Error>)null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("errors");
    }

    [Fact(DisplayName = "Failure(IEnumerable<Error>) → throws ArgumentException when the error collection is empty")]
    public void Failure_Should_ThrowArgumentException_When_ErrorCollectionIsEmpty()
    {
        Action act = () => Result.Failure([]);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("errors");
    }

    [Fact(DisplayName = "Failure(IEnumerable<Error>) → throws ArgumentException when the collection contains only null")]
    public void Failure_Should_ThrowArgumentException_When_ErrorCollectionContainsOnlyNullValues()
    {
        Action act = () => Result.Failure([null!]);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("errors");
    }

    [Fact(DisplayName = "Combine → throws ArgumentNullException when the results array is null")]
    public void Combine_Should_ThrowArgumentNullException_When_ResultsArrayIsNull()
    {
        Action act = () => Result.Combine(null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("results");
    }

    [Fact(DisplayName = "Combine → returns a successful result when the results array is empty")]
    public void Combine_Should_ReturnSuccessfulResult_When_ResultsArrayIsEmpty()
    {
        var result = Result.Combine();

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Combine → returns a successful result when all results are successful")]
    public void Combine_Should_ReturnSuccessfulResult_When_AllResultsAreSuccessful()
    {
        var first = Result.Success();
        var second = Result.Success();

        var result = Result.Combine(first, second);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Combine → returns a failure with aggregated errors when the results contain errors")]
    public void Combine_Should_ReturnFailureWithAggregatedErrors_When_AnyResultIsFailure()
    {
        var firstError = Error.Validation("Validation failed");
        var secondError = Error.NotFound("Not found");

        var first = Result.Success();
        var second = Result.Failure(firstError);
        var third = Result.Failure([secondError]);

        var result = Result.Combine(first, second, third);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Equal(firstError, secondError);
    }

    [Fact(DisplayName = "Result ctor → throws ArgumentException when a successful result contains errors")]
    public void Constructor_Should_ThrowArgumentException_When_SuccessResultContainsErrors()
    {
        var errors = new[]
        {
            Error.Validation("Validation failed")
        };

        Action act = () => _ = CreateResult(true, errors);

        var exception = act.Should().Throw<TargetInvocationException>()
            .Which.InnerException.Should().BeOfType<ArgumentException>()
            .Subject;

        exception.ParamName.Should().Be("errors");
    }

    [Fact(DisplayName = "Result ctor → throws ArgumentException when a failed result does not contain errors")]
    public void Constructor_Should_ThrowArgumentException_When_FailureResultDoesNotContainErrors()
    {
        var errors = Array.Empty<Error>();
        Action act = () => _ = CreateResult(false, errors);

        var exception = act.Should().Throw<TargetInvocationException>()
            .Which.InnerException.Should().BeOfType<ArgumentException>()
            .Subject;

        exception.ParamName.Should().Be("errors");
    }

    private static Result CreateResult(bool isSuccess, Error[] errors)
    {
        var constructor = typeof(Result).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            binder: null,
            [typeof(bool), typeof(Error[])],
            modifiers: null);

        constructor.Should().NotBeNull();

        return (Result)constructor.Invoke([isSuccess, errors]);
    }
}
