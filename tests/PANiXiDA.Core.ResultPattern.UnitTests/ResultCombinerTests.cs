namespace PANiXiDA.Core.ResultPattern.UnitTests;

public sealed class ResultCombinerTests
{
    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>) → returns a tuple result when both results are successful")]
    public void TupleCombine2_Should_ReturnTuple_When_AllResultsAreSuccessful()
    {
        var first = Result.Success(1);
        var second = Result.Success(2);

        var result = ResultCombiner.Combine(first, second);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be((1, 2));
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>) → throws ArgumentNullException when the first result is null")]
    public void TupleCombine2_Should_ThrowArgumentNullException_When_FirstIsNull()
    {
        Action act = () => ResultCombiner.Combine<int, int>(
            null!,
            Result.Success(2));

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("first");
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>) → returns a tuple result when all results are successful")]
    public void TupleCombine3_Should_ReturnTuple_When_AllResultsAreSuccessful()
    {
        var first = Result.Success(1);
        var second = Result.Success(2);
        var third = Result.Success(3);

        var result = ResultCombiner.Combine(first, second, third);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be((1, 2, 3));
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>) → returns a failure with aggregated errors when the results contain errors")]
    public void TupleCombine3_Should_ReturnFailure_When_AnyResultIsFailure()
    {
        var firstError = Error.Validation("first");
        var secondError = Error.Conflict("second");

        var first = Result.Failure<int>(firstError);
        var second = Result.Success(2);
        var third = Result.Failure<int>(secondError);

        var result = ResultCombiner.Combine(first, second, third);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Equal(firstError, secondError);
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Result<T4>) → returns a tuple result when all results are successful")]
    public void TupleCombine4_Should_ReturnTuple_When_AllResultsAreSuccessful()
    {
        var first = Result.Success(1);
        var second = Result.Success(2);
        var third = Result.Success(3);
        var fourth = Result.Success(4);

        var result = ResultCombiner.Combine(first, second, third, fourth);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be((1, 2, 3, 4));
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Result<T4>, Result<T5>) → returns a tuple result when all results are successful")]
    public void TupleCombine5_Should_ReturnTuple_When_AllResultsAreSuccessful()
    {
        var first = Result.Success(1);
        var second = Result.Success(2);
        var third = Result.Success(3);
        var fourth = Result.Success(4);
        var fifth = Result.Success(5);

        var result = ResultCombiner.Combine(first, second, third, fourth, fifth);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be((1, 2, 3, 4, 5));
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Result<T4>, Result<T5>, Result<T6>) → returns a tuple result when all results are successful")]
    public void TupleCombine6_Should_ReturnTuple_When_AllResultsAreSuccessful()
    {
        var first = Result.Success(1);
        var second = Result.Success(2);
        var third = Result.Success(3);
        var fourth = Result.Success(4);
        var fifth = Result.Success(5);
        var sixth = Result.Success(6);

        var result = ResultCombiner.Combine(first, second, third, fourth, fifth, sixth);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be((1, 2, 3, 4, 5, 6));
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Result<T4>, Result<T5>, Result<T6>, Result<T7>) → returns a tuple result preserving the original value order")]
    public void TupleCombine7_Should_ReturnTupleInOriginalOrder_When_AllResultsAreSuccessful()
    {
        var first = Result.Success("1");
        var second = Result.Success("2");
        var third = Result.Success("3");
        var fourth = Result.Success("4");
        var fifth = Result.Success("5");
        var sixth = Result.Success("6");
        var seventh = Result.Success("7");

        var result = ResultCombiner.Combine(first, second, third, fourth, fifth, sixth, seventh);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(("1", "2", "3", "4", "5", "6", "7"));
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Func<T1, T2, Result<TOut>>) → returns the bind result when both results are successful")]
    public void Combine2_Should_ReturnBindResult_When_AllResultsAreSuccessful()
    {
        var first = Result.Success(1);
        var second = Result.Success(2);

        var result = ResultCombiner.Combine(
            first,
            second,
            static (firstValue, secondValue) => Result.Success(firstValue + secondValue));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(3);
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Func<T1, T2, Result<TOut>>) → throws ArgumentNullException when the first result is null")]
    public void Combine2_Should_ThrowArgumentNullException_When_FirstIsNull()
    {
        Action act = () => ResultCombiner.Combine<int, int, int>(
            null!,
            Result.Success(2),
            static (first, second) => Result.Success(first + second));

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("first");
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Func<T1, T2, Result<TOut>>) → throws ArgumentNullException when bind is null")]
    public void Combine2_Should_ThrowArgumentNullException_When_BindIsNull()
    {
        Action act = () => ResultCombiner.Combine<int, int, int>(
            Result.Success(1),
            Result.Success(2),
            (Func<int, int, Result<int>>)null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("bind");
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Func<T1, T2, T3, Result<TOut>>) → returns the bind result when all results are successful")]
    public void Combine3_Should_ReturnBindResult_When_AllResultsAreSuccessful()
    {
        var first = Result.Success(1);
        var second = Result.Success(2);
        var third = Result.Success(3);

        var result = ResultCombiner.Combine(
            first,
            second,
            third,
            static (firstValue, secondValue, thirdValue) => Result.Success(firstValue + secondValue + thirdValue));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(6);
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Func<T1, T2, T3, Result<TOut>>) → returns failure without invoking bind when the results contain errors")]
    public void Combine3_Should_ReturnFailureWithoutInvokingBind_When_AnyResultIsFailure()
    {
        var firstError = Error.Validation("first");
        var secondError = Error.Conflict("second");
        var wasInvoked = false;

        var first = Result.Failure<int>(firstError);
        var second = Result.Success(2);
        var third = Result.Failure<int>(secondError);

        var result = ResultCombiner.Combine(
            first,
            second,
            third,
            (firstValue, secondValue, thirdValue) =>
            {
                wasInvoked = true;
                return Result.Success(firstValue + secondValue + thirdValue);
            });

        wasInvoked.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Equal(firstError, secondError);
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Result<T4>, Func<T1, T2, T3, T4, Result<TOut>>) → returns the bind result when all results are successful")]
    public void Combine4_Should_ReturnBindResult_When_AllResultsAreSuccessful()
    {
        var first = Result.Success(1);
        var second = Result.Success(2);
        var third = Result.Success(3);
        var fourth = Result.Success(4);

        var result = ResultCombiner.Combine(
            first,
            second,
            third,
            fourth,
            static (firstValue, secondValue, thirdValue, fourthValue) =>
                Result.Success(firstValue + secondValue + thirdValue + fourthValue));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(10);
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Result<T4>, Result<T5>, Func<T1, T2, T3, T4, T5, Result<TOut>>) → returns the bind result when all results are successful")]
    public void Combine5_Should_ReturnBindResult_When_AllResultsAreSuccessful()
    {
        var first = Result.Success(1);
        var second = Result.Success(2);
        var third = Result.Success(3);
        var fourth = Result.Success(4);
        var fifth = Result.Success(5);

        var result = ResultCombiner.Combine(
            first,
            second,
            third,
            fourth,
            fifth,
            static (firstValue, secondValue, thirdValue, fourthValue, fifthValue) =>
                Result.Success(firstValue + secondValue + thirdValue + fourthValue + fifthValue));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(15);
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Result<T4>, Result<T5>, Result<T6>, Func<T1, T2, T3, T4, T5, T6, Result<TOut>>) → returns the bind result when all results are successful")]
    public void Combine6_Should_ReturnBindResult_When_AllResultsAreSuccessful()
    {
        var first = Result.Success(1);
        var second = Result.Success(2);
        var third = Result.Success(3);
        var fourth = Result.Success(4);
        var fifth = Result.Success(5);
        var sixth = Result.Success(6);

        var result = ResultCombiner.Combine(
            first,
            second,
            third,
            fourth,
            fifth,
            sixth,
            static (firstValue, secondValue, thirdValue, fourthValue, fifthValue, sixthValue) =>
                Result.Success(firstValue + secondValue + thirdValue + fourthValue + fifthValue + sixthValue));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(21);
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Result<T4>, Result<T5>, Result<T6>, Result<T7>, Func<T1, T2, T3, T4, T5, T6, T7, Result<TOut>>) → passes values to bind in the original order")]
    public void Combine7_Should_PassValuesInOrderToBind_When_AllResultsAreSuccessful()
    {
        var first = Result.Success("1");
        var second = Result.Success("2");
        var third = Result.Success("3");
        var fourth = Result.Success("4");
        var fifth = Result.Success("5");
        var sixth = Result.Success("6");
        var seventh = Result.Success("7");

        var result = ResultCombiner.Combine(
            first,
            second,
            third,
            fourth,
            fifth,
            sixth,
            seventh,
            static (firstValue, secondValue, thirdValue, fourthValue, fifthValue, sixthValue, seventhValue) =>
            {
                return Result.Success(string.Concat(
                    firstValue,
                    secondValue,
                    thirdValue,
                    fourthValue,
                    fifthValue,
                    sixthValue,
                    seventhValue));
            });

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("1234567");
    }

    [Fact(DisplayName = "Combine(Result<T1>, Result<T2>, Result<T3>, Result<T4>, Result<T5>, Result<T6>, Result<T7>, Func<T1, T2, T3, T4, T5, T6, T7, Result<TOut>>) → returns failure without invoking bind when the results contain errors")]
    public void Combine7_Should_ReturnFailureWithoutInvokingBind_When_AnyResultIsFailure()
    {
        var firstError = Error.Validation("first");
        var secondError = Error.NotFound("second");
        var thirdError = Error.Conflict("third");
        var wasInvoked = false;

        var first = Result.Success(1);
        var second = Result.Failure<int>(firstError);
        var third = Result.Success(3);
        var fourth = Result.Failure<int>(secondError);
        var fifth = Result.Success(5);
        var sixth = Result.Failure<int>(thirdError);
        var seventh = Result.Success(7);

        var result = ResultCombiner.Combine(
            first,
            second,
            third,
            fourth,
            fifth,
            sixth,
            seventh,
            (firstValue, secondValue, thirdValue, fourthValue, fifthValue, sixthValue, seventhValue) =>
            {
                wasInvoked = true;
                return Result.Success(
                    firstValue +
                    secondValue +
                    thirdValue +
                    fourthValue +
                    fifthValue +
                    sixthValue +
                    seventhValue);
            });

        wasInvoked.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Equal(firstError, secondError, thirdError);
    }
}
