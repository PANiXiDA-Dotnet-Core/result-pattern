namespace PANiXiDA.Core.ResultPattern.UnitTests;

public sealed class ResultExtensionsTests
{
    [Fact(DisplayName = "Map(Result, Func<TOut>) → throws ArgumentNullException when result is null")]
    public void Map_Should_ThrowArgumentNullException_When_ResultIsNull()
    {
        Action act = () => ResultExtensions.Map<int>(null!, () => 1);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "Map(Result, Func<TOut>) → throws ArgumentNullException when map is null")]
    public void Map_Should_ThrowArgumentNullException_When_MapIsNull()
    {
        var result = Result.Success();
        Action act = () => result.Map((Func<int>)null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("map");
    }

    [Fact(DisplayName = "Map(Result, Func<TOut>) → returns mapped result when the source result is successful")]
    public void Map_Should_ReturnMappedResult_When_ResultIsSuccess()
    {
        const int expectedValue = 42;
        var result = Result.Success();

        var mappedResult = result.Map(() => expectedValue);

        mappedResult.IsSuccess.Should().BeTrue();
        mappedResult.Value.Should().Be(expectedValue);
    }

    [Fact(DisplayName = "Map(Result, Func<TOut>) → returns failure without invoking mapper when the source result is a failure")]
    public void Map_Should_ReturnFailureWithoutInvokingMapper_When_ResultIsFailure()
    {
        var sourceError = Error.Validation("Validation failed");
        var result = Result.Failure(sourceError);
        var wasInvoked = false;

        var mappedResult = result.Map(() =>
        {
            wasInvoked = true;
            return 42;
        });

        wasInvoked.Should().BeFalse();
        mappedResult.IsFailure.Should().BeTrue();
        mappedResult.Errors.Should().ContainSingle().Which.Should().BeSameAs(sourceError);
    }

    [Fact(DisplayName = "Map(Result<TIn>, Func<TIn, TOut>) → throws ArgumentNullException when result is null")]
    public void Map_Should_ThrowArgumentNullException_When_GenericResultIsNull()
    {
        Action act = () => ResultExtensions.Map<string, int>(null!, value => value.Length);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "Map(Result<TIn>, Func<TIn, TOut>) → throws ArgumentNullException when map is null")]
    public void Map_Should_ThrowArgumentNullException_When_GenericMapIsNull()
    {
        var result = Result.Success("value");
        Action act = () => result.Map((Func<string, int>)null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("map");
    }

    [Fact(DisplayName = "Map(Result<TIn>, Func<TIn, TOut>) → returns mapped result when the source generic result is successful")]
    public void Map_Should_ReturnMappedResult_When_GenericResultIsSuccess()
    {
        const string value = "value";
        var result = Result.Success(value);

        var mappedResult = result.Map(item => item.Length);

        mappedResult.IsSuccess.Should().BeTrue();
        mappedResult.Value.Should().Be(value.Length);
    }

    [Fact(DisplayName = "Map(Result<TIn>, Func<TIn, TOut>) → returns failure without invoking mapper when the source generic result is a failure")]
    public void Map_Should_ReturnFailureWithoutInvokingMapper_When_GenericResultIsFailure()
    {
        var sourceError = Error.Validation("Validation failed");
        var result = Result.Failure<string>(sourceError);
        var wasInvoked = false;

        var mappedResult = result.Map(value =>
        {
            wasInvoked = true;
            return value.Length;
        });

        wasInvoked.Should().BeFalse();
        mappedResult.IsFailure.Should().BeTrue();
        mappedResult.Errors.Should().ContainSingle().Which.Should().BeSameAs(sourceError);
    }

    [Fact(DisplayName = "Bind(Result<TIn>, Func<TIn, Result>) → throws ArgumentNullException when result is null")]
    public void Bind_Should_ThrowArgumentNullException_When_ResultIsNull()
    {
        Action act = () => ResultExtensions.Bind<string>(null!, value => Result.Success());

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "Bind(Result<TIn>, Func<TIn, Result>) → throws ArgumentNullException when bind is null")]
    public void Bind_Should_ThrowArgumentNullException_When_BindIsNull()
    {
        var result = Result.Success("value");
        Action act = () => result.Bind((Func<string, Result>)null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("bind");
    }

    [Fact(DisplayName = "Bind(Result<TIn>, Func<TIn, Result>) → returns the bind result when the source generic result is successful")]
    public void Bind_Should_ReturnBindResult_When_ResultIsSuccess()
    {
        const string value = "value";
        var result = Result.Success(value);

        var boundResult = result.Bind(item =>
        {
            return item.Length > 0
                ? Result.Success()
                : Result.Failure(Error.Validation("Validation failed"));
        });

        boundResult.IsSuccess.Should().BeTrue();
    }

    [Fact(DisplayName = "Bind(Result<TIn>, Func<TIn, Result>) → returns failure without invoking bind when the source generic result is a failure")]
    public void Bind_Should_ReturnFailureWithoutInvokingBind_When_ResultIsFailure()
    {
        var sourceError = Error.Validation("Validation failed");
        var result = Result.Failure<string>(sourceError);
        var wasInvoked = false;

        var boundResult = result.Bind(_ =>
        {
            wasInvoked = true;
            return Result.Success();
        });

        wasInvoked.Should().BeFalse();
        boundResult.IsFailure.Should().BeTrue();
        boundResult.Errors.Should().ContainSingle().Which.Should().BeSameAs(sourceError);
    }

    [Fact(DisplayName = "Bind(Result<TIn>, Func<TIn, Result<TOut>>) → throws ArgumentNullException when result is null")]
    public void Bind_Should_ThrowArgumentNullException_When_GenericResultIsNull()
    {
        Action act = () => ResultExtensions.Bind<string, int>(null!, value => Result.Success(value.Length));

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "Bind(Result<TIn>, Func<TIn, Result<TOut>>) → throws ArgumentNullException when bind is null")]
    public void Bind_Should_ThrowArgumentNullException_When_GenericBindIsNull()
    {
        var result = Result.Success("value");
        Action act = () => result.Bind((Func<string, Result<int>>)null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("bind");
    }

    [Fact(DisplayName = "Bind(Result<TIn>, Func<TIn, Result<TOut>>) → returns the bind result when the source generic result is successful")]
    public void Bind_Should_ReturnBindResult_When_GenericResultIsSuccess()
    {
        const string value = "value";
        var result = Result.Success(value);

        var boundResult = result.Bind(item => Result.Success(item.Length));

        boundResult.IsSuccess.Should().BeTrue();
        boundResult.Value.Should().Be(value.Length);
    }

    [Fact(DisplayName = "Bind(Result<TIn>, Func<TIn, Result<TOut>>) → returns failure without invoking bind when the source generic result is a failure")]
    public void Bind_Should_ReturnFailureWithoutInvokingBind_When_GenericResultIsFailure()
    {
        var sourceError = Error.Validation("Validation failed");
        var result = Result.Failure<string>(sourceError);
        var wasInvoked = false;

        var boundResult = result.Bind(value =>
        {
            wasInvoked = true;
            return Result.Success(value.Length);
        });

        wasInvoked.Should().BeFalse();
        boundResult.IsFailure.Should().BeTrue();
        boundResult.Errors.Should().ContainSingle().Which.Should().BeSameAs(sourceError);
    }

    [Fact(DisplayName = "Bind(Result, Func<Result<TOut>>) → throws ArgumentNullException when result is null")]
    public void Bind_Should_ThrowArgumentNullException_When_NonGenericResultIsNull()
    {
        Action act = () => ResultExtensions.Bind<int>(null!, () => Result.Success(42));

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "Bind(Result, Func<Result<TOut>>) → throws ArgumentNullException when bind is null")]
    public void Bind_Should_ThrowArgumentNullException_When_NonGenericBindIsNull()
    {
        var result = Result.Success();
        Action act = () => result.Bind((Func<Result<int>>)null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("bind");
    }

    [Fact(DisplayName = "Bind(Result, Func<Result<TOut>>) → returns the bind result when the source result is successful")]
    public void Bind_Should_ReturnBindResult_When_NonGenericResultIsSuccess()
    {
        const int expectedValue = 42;
        var result = Result.Success();

        var boundResult = result.Bind(() => Result.Success(expectedValue));

        boundResult.IsSuccess.Should().BeTrue();
        boundResult.Value.Should().Be(expectedValue);
    }

    [Fact(DisplayName = "Bind(Result, Func<Result<TOut>>) → returns failure without invoking bind when the source result is a failure")]
    public void Bind_Should_ReturnFailureWithoutInvokingBind_When_NonGenericResultIsFailure()
    {
        var sourceError = Error.Validation("Validation failed");
        var result = Result.Failure(sourceError);
        var wasInvoked = false;

        var boundResult = result.Bind(() =>
        {
            wasInvoked = true;
            return Result.Success(42);
        });

        wasInvoked.Should().BeFalse();
        boundResult.IsFailure.Should().BeTrue();
        boundResult.Errors.Should().ContainSingle().Which.Should().BeSameAs(sourceError);
    }

    [Fact(DisplayName = "BindAsync(Result<TIn>, Func<TIn, Task<Result<TOut>>>) → throws ArgumentNullException when result is null")]
    public async Task BindAsync_Should_ThrowArgumentNullException_When_ResultIsNull()
    {
        Func<Task> act = async () => _ = await ResultExtensions.BindAsync<string, int>(
            null!,
            value => Task.FromResult(Result.Success(value.Length)));

        var exception = (await act.Should().ThrowAsync<ArgumentNullException>()).Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "BindAsync(Result<TIn>, Func<TIn, Task<Result<TOut>>>) → throws ArgumentNullException when bind is null")]
    public async Task BindAsync_Should_ThrowArgumentNullException_When_BindIsNull()
    {
        var result = Result.Success("value");
        Func<Task> act = async () => _ = await result.BindAsync((Func<string, Task<Result<int>>>)null!);

        var exception = (await act.Should().ThrowAsync<ArgumentNullException>()).Which;

        exception.ParamName.Should().Be("bind");
    }

    [Fact(DisplayName = "BindAsync(Result<TIn>, Func<TIn, Task<Result<TOut>>>) → returns the bind result when the source generic result is successful")]
    public async Task BindAsync_Should_ReturnBindResult_When_ResultIsSuccess()
    {
        const string value = "value";
        var result = Result.Success(value);

        var boundResult = await result.BindAsync(item => Task.FromResult(Result.Success(item.Length)));

        boundResult.IsSuccess.Should().BeTrue();
        boundResult.Value.Should().Be(value.Length);
    }

    [Fact(DisplayName = "BindAsync(Result<TIn>, Func<TIn, Task<Result<TOut>>>) → returns failure without invoking bind when the source generic result is a failure")]
    public async Task BindAsync_Should_ReturnFailureWithoutInvokingBind_When_ResultIsFailure()
    {
        var sourceError = Error.Validation("Validation failed");
        var result = Result.Failure<string>(sourceError);
        var wasInvoked = false;

        var boundResult = await result.BindAsync(value =>
        {
            wasInvoked = true;
            return Task.FromResult(Result.Success(value.Length));
        });

        wasInvoked.Should().BeFalse();
        boundResult.IsFailure.Should().BeTrue();
        boundResult.Errors.Should().ContainSingle().Which.Should().BeSameAs(sourceError);
    }

    [Fact(DisplayName = "BindAsync(Result, Func<Task<Result<TOut>>>) → throws ArgumentNullException when result is null")]
    public async Task BindAsync_Should_ThrowArgumentNullException_When_NonGenericResultIsNull()
    {
        Func<Task> act = async () => _ = await ResultExtensions.BindAsync<int>(
            null!,
            () => Task.FromResult(Result.Success(42)));

        var exception = (await act.Should().ThrowAsync<ArgumentNullException>()).Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "BindAsync(Result, Func<Task<Result<TOut>>>) → throws ArgumentNullException when bind is null")]
    public async Task BindAsync_Should_ThrowArgumentNullException_When_NonGenericBindIsNull()
    {
        var result = Result.Success();
        Func<Task> act = async () => _ = await result.BindAsync((Func<Task<Result<int>>>)null!);

        var exception = (await act.Should().ThrowAsync<ArgumentNullException>()).Which;

        exception.ParamName.Should().Be("bind");
    }

    [Fact(DisplayName = "BindAsync(Result, Func<Task<Result<TOut>>>) → returns the bind result when the source result is successful")]
    public async Task BindAsync_Should_ReturnBindResult_When_NonGenericResultIsSuccess()
    {
        const int expectedValue = 42;
        var result = Result.Success();

        var boundResult = await result.BindAsync(() => Task.FromResult(Result.Success(expectedValue)));

        boundResult.IsSuccess.Should().BeTrue();
        boundResult.Value.Should().Be(expectedValue);
    }

    [Fact(DisplayName = "BindAsync(Result, Func<Task<Result<TOut>>>) → returns failure without invoking bind when the source result is a failure")]
    public async Task BindAsync_Should_ReturnFailureWithoutInvokingBind_When_NonGenericResultIsFailure()
    {
        var sourceError = Error.Validation("Validation failed");
        var result = Result.Failure(sourceError);
        var wasInvoked = false;

        var boundResult = await result.BindAsync(() =>
        {
            wasInvoked = true;
            return Task.FromResult(Result.Success(42));
        });

        wasInvoked.Should().BeFalse();
        boundResult.IsFailure.Should().BeTrue();
        boundResult.Errors.Should().ContainSingle().Which.Should().BeSameAs(sourceError);
    }

    [Fact(DisplayName = "Ensure → throws ArgumentNullException when result is null")]
    public void Ensure_Should_ThrowArgumentNullException_When_ResultIsNull()
    {
        Action act = () => ResultExtensions.Ensure<string>(
            null!,
            _ => true,
            Error.Validation("Validation failed"));

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "Ensure → throws ArgumentNullException when predicate is null")]
    public void Ensure_Should_ThrowArgumentNullException_When_PredicateIsNull()
    {
        var result = Result.Success("value");
        Action act = () => result.Ensure(null!, Error.Validation("Validation failed"));

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("predicate");
    }

    [Fact(DisplayName = "Ensure → throws ArgumentNullException when error is null")]
    public void Ensure_Should_ThrowArgumentNullException_When_ErrorIsNull()
    {
        var result = Result.Success("value");
        Action act = () => result.Ensure(_ => true, null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("error");
    }

    [Fact(DisplayName = "Ensure → returns the original result when the source result is already a failure")]
    public void Ensure_Should_ReturnOriginalResult_When_ResultIsFailure()
    {
        var result = Result.Failure<string>(Error.Validation("Validation failed"));
        var wasInvoked = false;

        var ensuredResult = result.Ensure(
            _ =>
            {
                wasInvoked = true;
                return true;
            },
            Error.Conflict("Conflict"));

        wasInvoked.Should().BeFalse();
        ensuredResult.Should().BeSameAs(result);
    }

    [Fact(DisplayName = "Ensure → returns the original result when predicate returns true")]
    public void Ensure_Should_ReturnOriginalResult_When_PredicateReturnsTrue()
    {
        const string value = "value";
        var result = Result.Success(value);

        var ensuredResult = result.Ensure(
            item => item.Length == value.Length,
            Error.Validation("Validation failed"));

        ensuredResult.Should().BeSameAs(result);
    }

    [Fact(DisplayName = "Ensure → returns failure when predicate returns false")]
    public void Ensure_Should_ReturnFailure_When_PredicateReturnsFalse()
    {
        var error = Error.Validation("Validation failed");
        const string value = "value";
        const int invalidLength = 10;
        var result = Result.Success(value);

        var ensuredResult = result.Ensure(item => item.Length == invalidLength, error);

        ensuredResult.IsFailure.Should().BeTrue();
        ensuredResult.Errors.Should().ContainSingle().Which.Should().BeSameAs(error);
    }

    [Fact(DisplayName = "Tap → throws ArgumentNullException when result is null")]
    public void Tap_Should_ThrowArgumentNullException_When_ResultIsNull()
    {
        Action act = () => ResultExtensions.Tap<string>(null!, _ => { });

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "Tap → throws ArgumentNullException when action is null")]
    public void Tap_Should_ThrowArgumentNullException_When_ActionIsNull()
    {
        var result = Result.Success("value");
        Action act = () => result.Tap(null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("action");
    }

    [Fact(DisplayName = "Tap → executes action and returns the original result when the result is successful")]
    public void Tap_Should_InvokeActionAndReturnOriginalResult_When_ResultIsSuccess()
    {
        const string value = "value";
        var result = Result.Success(value);
        string? tappedValue = null;

        var tappedResult = result.Tap(item => tappedValue = item);

        tappedValue.Should().Be(value);
        tappedResult.Should().BeSameAs(result);
    }

    [Fact(DisplayName = "Tap → returns the original result without invoking action when the result is a failure")]
    public void Tap_Should_ReturnOriginalResultWithoutInvokingAction_When_ResultIsFailure()
    {
        var result = Result.Failure<string>(Error.Validation("Validation failed"));
        var wasInvoked = false;

        var tappedResult = result.Tap(_ => wasInvoked = true);

        wasInvoked.Should().BeFalse();
        tappedResult.Should().BeSameAs(result);
    }

    [Fact(DisplayName = "Match(Result, Func<TOut>, Func<IReadOnlyList<Error>, TOut>) → throws ArgumentNullException when result is null")]
    public void Match_Should_ThrowArgumentNullException_When_ResultIsNull()
    {
        Action act = () => ResultExtensions.Match(null!, () => 42, _ => 0);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "Match(Result, Func<TOut>, Func<IReadOnlyList<Error>, TOut>) → throws ArgumentNullException when onSuccess is null")]
    public void Match_Should_ThrowArgumentNullException_When_OnSuccessIsNull()
    {
        var result = Result.Success();
        Action act = () => result.Match((Func<int>)null!, _ => 0);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("onSuccess");
    }

    [Fact(DisplayName = "Match(Result, Func<TOut>, Func<IReadOnlyList<Error>, TOut>) → throws ArgumentNullException when onFailure is null")]
    public void Match_Should_ThrowArgumentNullException_When_OnFailureIsNull()
    {
        var result = Result.Success();
        Action act = () => result.Match(() => 42, null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("onFailure");
    }

    [Fact(DisplayName = "Match(Result, Func<TOut>, Func<IReadOnlyList<Error>, TOut>) → invokes onSuccess when the result is successful")]
    public void Match_Should_InvokeOnSuccess_When_ResultIsSuccess()
    {
        const int expectedValue = 42;
        var result = Result.Success();
        var onFailureInvoked = false;

        var matchResult = result.Match(
            () => expectedValue,
            _ =>
            {
                onFailureInvoked = true;
                return 0;
            });

        matchResult.Should().Be(expectedValue);
        onFailureInvoked.Should().BeFalse();
    }

    [Fact(DisplayName = "Match(Result, Func<TOut>, Func<IReadOnlyList<Error>, TOut>) → invokes onFailure with errors when the result is a failure")]
    public void Match_Should_InvokeOnFailureWithErrors_When_ResultIsFailure()
    {
        var error = Error.Validation("Validation failed");
        var result = Result.Failure(error);
        var onSuccessInvoked = false;

        var matchResult = result.Match(
            () =>
            {
                onSuccessInvoked = true;
                return 42;
            },
            errors =>
            {
                errors.Should().ContainSingle().Which.Should().BeSameAs(error);
                return 0;
            });

        matchResult.Should().Be(0);
        onSuccessInvoked.Should().BeFalse();
    }

    [Fact(DisplayName = "Match(Result<TIn>, Func<TIn, TOut>, Func<IReadOnlyList<Error>, TOut>) → throws ArgumentNullException when result is null")]
    public void Match_Should_ThrowArgumentNullException_When_GenericResultIsNull()
    {
        Action act = () => ResultExtensions.Match<string, int>(null!, _ => 42, _ => 0);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("result");
    }

    [Fact(DisplayName = "Match(Result<TIn>, Func<TIn, TOut>, Func<IReadOnlyList<Error>, TOut>) → throws ArgumentNullException when onSuccess is null")]
    public void Match_Should_ThrowArgumentNullException_When_GenericOnSuccessIsNull()
    {
        var result = Result.Success("value");
        Action act = () => result.Match((Func<string, int>)null!, _ => 0);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("onSuccess");
    }

    [Fact(DisplayName = "Match(Result<TIn>, Func<TIn, TOut>, Func<IReadOnlyList<Error>, TOut>) → throws ArgumentNullException when onFailure is null")]
    public void Match_Should_ThrowArgumentNullException_When_GenericOnFailureIsNull()
    {
        var result = Result.Success("value");
        Action act = () => result.Match(_ => 42, null!);

        var exception = act.Should().Throw<ArgumentNullException>().Which;

        exception.ParamName.Should().Be("onFailure");
    }

    [Fact(DisplayName = "Match(Result<TIn>, Func<TIn, TOut>, Func<IReadOnlyList<Error>, TOut>) → invokes onSuccess with the value when the result is successful")]
    public void Match_Should_InvokeOnSuccessWithValue_When_GenericResultIsSuccess()
    {
        const string value = "value";
        var result = Result.Success(value);
        var onFailureInvoked = false;

        var matchResult = result.Match(
            item => item.Length,
            _ =>
            {
                onFailureInvoked = true;
                return 0;
            });

        matchResult.Should().Be(value.Length);
        onFailureInvoked.Should().BeFalse();
    }

    [Fact(DisplayName = "Match(Result<TIn>, Func<TIn, TOut>, Func<IReadOnlyList<Error>, TOut>) → invokes onFailure with errors when the result is a failure")]
    public void Match_Should_InvokeOnFailureWithErrors_When_GenericResultIsFailure()
    {
        var error = Error.Validation("Validation failed");
        var result = Result.Failure<string>(error);
        var onSuccessInvoked = false;

        var matchResult = result.Match(
            _ =>
            {
                onSuccessInvoked = true;
                return 42;
            },
            errors =>
            {
                errors.Should().ContainSingle().Which.Should().BeSameAs(error);
                return 0;
            });

        matchResult.Should().Be(0);
        onSuccessInvoked.Should().BeFalse();
    }
}
