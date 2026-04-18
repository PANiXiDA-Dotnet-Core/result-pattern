# PANiXiDA.Core.ResultPattern

`PANiXiDA.Core.ResultPattern` is a small .NET library for explicit success and failure handling in business logic without using exceptions as the primary control-flow contract.

It is designed for .NET developers who want predictable result-based workflows, typed errors, and composable synchronous and asynchronous operation pipelines.

## Status

[![CI](https://github.com/PANiXiDA-Dotnet-Core/result-pattern/actions/workflows/ci.yml/badge.svg)](https://github.com/PANiXiDA-Dotnet-Core/result-pattern/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/PANiXiDA.Core.ResultPattern.svg)](https://www.nuget.org/packages/PANiXiDA.Core.ResultPattern)
[![NuGet downloads](https://img.shields.io/nuget/dt/PANiXiDA.Core.ResultPattern.svg)](https://www.nuget.org/packages/PANiXiDA.Core.ResultPattern)
[![License](https://img.shields.io/github/license/PANiXiDA-Dotnet-Core/result-pattern.svg)](LICENSE)

## Overview

When a method in business logic can end not only with success but also with an expected failure, exceptions often become an awkward contract:

- the method signature does not show that the method can fail;
- business errors get mixed with technical exceptions;
- the code starts to grow with `try/catch` blocks;
- composing multiple steps becomes harder to read.

`PANiXiDA.Core.ResultPattern` addresses this by making operation outcomes explicit:

- `Result` represents success or failure without a value;
- `Result<T>` represents success or failure with a value;
- `Error` provides a unified error model with type, message, and metadata;
- extension methods such as `Map`, `Bind`, `BindAsync`, `Ensure`, `Tap`, and `Match` help compose operation pipelines.

This library is especially useful in:

- application services;
- use cases;
- orchestrator layers;
- domain factories and validators;
- API boundaries.

## Features

- Explicit success/failure contract with `Result` and `Result<T>`
- Typed error model through `Error` and `ErrorType`
- Support for both single and multiple errors
- Synchronous and asynchronous pipeline composition
- Validation-style workflow support with error aggregation
- Lightweight public API
- XML-documented public API surface
- Suitable for application, domain, and API boundary layers

## Quick Start

### Requirements

- .NET 10 SDK

### Installation

The library targets `net10.0`.

```xml
<ItemGroup>
  <PackageReference Include="PANiXiDA.Core.ResultPattern" Version="..." />
</ItemGroup>
```

### Minimal import

```csharp
using PANiXiDA.Core.ResultPattern;
```

### First example

```csharp
using PANiXiDA.Core.ResultPattern;

Result<string> GetUserName(bool exists)
{
    if (!exists)
    {
        return Result.Failure<string>(Error.NotFound("User not found"));
    }

    return Result.Success("John");
}

var result = GetUserName(exists: true);

if (result.IsSuccess)
{
    Console.WriteLine(result.Value);
}
```

## Usage

### Creating errors

```csharp
using PANiXiDA.Core.ResultPattern;

var validationError = Error.Validation("Email is required");
var notFoundError = Error.NotFound("User not found");
var conflictError = Error.Conflict("Email is already in use");
var forbiddenError = Error.Forbidden("Insufficient permissions");

var fieldError = Error.Validation("Invalid email format")
    .WithField("email")
    .WithMetadata("attemptedValue", "not-an-email");
```

### `Result` without a value

```csharp
using PANiXiDA.Core.ResultPattern;

Result DeleteUser(bool userExists)
{
    if (!userExists)
    {
        return Result.Failure(Error.NotFound("User not found"));
    }

    return Result.Success();
}
```

### `Result<T>` with a value

```csharp
using PANiXiDA.Core.ResultPattern;

public sealed record UserDto(Guid Id, string Email);

Result<UserDto> GetUser(Guid id, UserDto? user)
{
    if (user is null)
    {
        return Result.Failure<UserDto>(Error.NotFound("User not found"));
    }

    return Result.Success(user);
}
```

### Checking `IsSuccess` / `IsFailure` and reading `FirstError`

```csharp
var result = DeleteUser(userExists: false);

if (result.IsFailure)
{
    Console.WriteLine(result.FirstError.Message);
}
```

### `Value`, `ValueOrDefault`, and `TryGetValue`

```csharp
var userResult = GetUser(Guid.NewGuid(), new UserDto(Guid.NewGuid(), "user@example.com"));

var value = userResult.Value;
var sameValue = userResult.ValueOrDefault;

if (userResult.TryGetValue(out var user))
{
    Console.WriteLine(user.Email);
}
```

```csharp
var failedResult = Result.Failure<UserDto>(Error.NotFound("User not found"));

var defaultValue = failedResult.ValueOrDefault;
var hasValue = failedResult.TryGetValue(out var missingUser);

Console.WriteLine(defaultValue is null); // True
Console.WriteLine(hasValue);             // False
Console.WriteLine(missingUser is null);  // True
```

### Returning multiple errors

```csharp
Result ValidateRegistration(string email, string password)
{
    var errors = new List<Error>();

    if (string.IsNullOrWhiteSpace(email))
    {
        errors.Add(Error.Validation("Email is required").WithField("email"));
    }

    if (string.IsNullOrWhiteSpace(password))
    {
        errors.Add(Error.Validation("Password is required").WithField("password"));
    }

    if (errors.Count > 0)
    {
        return Result.Failure(errors);
    }

    return Result.Success();
}
```

### `Combine` for joining multiple validations

```csharp
var emailValidation = ValidateEmail(email);
var passwordValidation = ValidatePassword(password);
var agreementValidation = ValidateAgreement(agreementAccepted);

var validationResult = Result.Combine(
    emailValidation,
    passwordValidation,
    agreementValidation);

if (validationResult.IsFailure)
{
    return validationResult;
}
```

### `Map` for transforming a result

`Map` is useful when the source operation is already successful and you only need to transform the value.

```csharp
Result validationResult = ValidateRegistration(email, password);
Result<Guid> requestIdResult = validationResult.Map(() => Guid.NewGuid());
```

```csharp
public sealed record User(Guid Id, string Email);
public sealed record UserResponse(Guid Id, string Email);

Result<User> userResult = Result.Success(new User(Guid.NewGuid(), "user@example.com"));

Result<UserResponse> responseResult = userResult.Map(user =>
{
    return new UserResponse(user.Id, user.Email);
});
```

### `Bind` for composing steps that already return `Result`

`Bind` is useful when the next step can also fail.

```csharp
Result validationResult = ValidateRegistration(email, password);
Result<Guid> createUserResult = validationResult.Bind(() =>
{
    return CreateUser(email, password);
});
```

```csharp
Result<User> userResult = GetUserById(userId);
Result activationResult = userResult.Bind(ActivateUser);
```

```csharp
Result<User> userResult = GetUserById(userId);

Result<UserResponse> responseResult = userResult.Bind(user =>
{
    return LoadProfile(user.Id).Map(profile =>
    {
        return new UserResponse(user.Id, user.Email);
    });
});
```

### `BindAsync` for asynchronous composition

```csharp
Result validationResult = ValidateRegistration(email, password);
Result<Guid> createUserResult = await validationResult.BindAsync(() =>
{
    return CreateUserAsync(email, password);
});
```

```csharp
Result<User> userResult = await GetUserByIdAsync(userId);

Result<UserResponse> responseResult = await userResult.BindAsync(async user =>
{
    var profileResult = await LoadProfileAsync(user.Id);

    return profileResult.Map(profile =>
    {
        return new UserResponse(user.Id, user.Email);
    });
});
```

### `Ensure` for additional checks after success

```csharp
Result<User> userResult = GetUserById(userId);

Result<User> activeUserResult = userResult
    .Ensure(
        user => user.IsActive,
        Error.Forbidden("User is blocked"))
    .Ensure(
        user => user.EmailConfirmed,
        Error.Validation("Email is not confirmed").WithField("email"));
```

### `Tap` for side effects

`Tap` does not change the result and is useful for logging, auditing, metrics, and other side effects.

```csharp
Result<User> createResult = CreateUser(email, password);

Result<User> sameResult = createResult.Tap(user =>
{
    Console.WriteLine($"User created: {user.Id}");
});
```

### `Match` for finishing the pipeline

`Match` is convenient at the application boundary, when you need to choose the final behavior for success and failure.

```csharp
Result<UserResponse> result = GetUserById(userId)
    .Map(user =>
    {
        return new UserResponse(user.Id, user.Email);
    });

var response = result.Match(
    onSuccess: user =>
    {
        return $"200 OK: {user.Email}";
    },
    onFailure: errors =>
    {
        return $"400/404: {string.Join("; ", errors.Select(error => error.Message))}";
    });
```

```csharp
Result deleteResult = DeleteUser(userExists: false);

var message = deleteResult.Match(
    onSuccess: () =>
    {
        return "User deleted";
    },
    onFailure: errors =>
    {
        return $"Deletion failed: {errors[0].Message}";
    });
```

### Full pipeline example

```csharp
using PANiXiDA.Core.ResultPattern;

public sealed record RegisterUserCommand(string Email, string Password);
public sealed record User(Guid Id, string Email, bool IsActive, bool EmailConfirmed);
public sealed record UserResponse(Guid Id, string Email);

public async Task<Result<UserResponse>> RegisterAsync(RegisterUserCommand command)
{
    var validationResult = ValidateRegistration(command.Email, command.Password);
    var uniqueEmailResult = validationResult.Bind(() =>
    {
        return EnsureEmailIsUnique(command.Email);
    });

    if (uniqueEmailResult.IsFailure)
    {
        return Result.Failure<UserResponse>(uniqueEmailResult.Errors);
    }

    var createResult = await uniqueEmailResult.BindAsync(() =>
    {
        return CreateUserAsync(command);
    });

    var guardedResult = createResult
        .Ensure(user => user.IsActive, Error.Failure("User was created in an inconsistent state"))
        .Ensure(user => user.EmailConfirmed, Error.Validation("Email is not confirmed").WithField("email"))
        .Tap(user =>
        {
            Console.WriteLine($"Created user {user.Id}");
        });

    return guardedResult.Map(user =>
    {
        return new UserResponse(user.Id, user.Email);
    });
}
```

### API boundary example

```csharp
public async Task<IResult> Register(RegisterUserCommand command)
{
    var result = await RegisterAsync(command);

    return result.Match<IResult>(
        onSuccess: user =>
        {
            return Results.Ok(user);
        },
        onFailure: errors =>
        {
            var firstError = errors[0];

            return firstError.Type switch
            {
                ErrorType.Validation => Results.BadRequest(errors),
                ErrorType.NotFound => Results.NotFound(errors),
                ErrorType.Conflict => Results.Conflict(errors),
                ErrorType.Unauthorized => Results.Unauthorized(),
                ErrorType.Forbidden => Results.StatusCode(StatusCodes.Status403Forbidden),
                _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
            };
        });
}
```

## Configuration

This library does not require runtime configuration.

There are no required:

* environment variables;
* `appsettings.json` entries;
* secrets;
* ports;
* external services.

The only consumer-side requirement is referencing the package from a compatible .NET project.

## Project Structure

```text
.
├── src/
│   └── PANiXiDA.Core.ResultPattern/
│       └── PANiXiDA.Core.ResultPattern.csproj
├── tests/
│   └── PANiXiDA.Core.ResultPattern.UnitTests/
│       └── PANiXiDA.Core.ResultPattern.UnitTests.csproj
├── .editorconfig
├── .gitattributes
├── .gitignore
├── Directory.Build.props
├── Directory.Packages.props
├── global.json
├── version.json
├── LICENSE
└── README.md
```

### Main repository files

* `src/` — library source code
* `tests/` — automated tests
* `Directory.Build.props` — shared MSBuild settings
* `Directory.Packages.props` — centralized package versions
* `global.json` — SDK and test runner configuration
* `version.json` — Nerdbank.GitVersioning configuration
* `.editorconfig` — code style rules
* `README.md` — package overview and usage documentation

## Development

### Build

```bash
dotnet restore
dotnet build --configuration Release
```

### Format

```bash
dotnet format
```

### Test

```bash
dotnet test --configuration Release
```

### Full local validation

```bash
dotnet restore
dotnet format
dotnet build --configuration Release
dotnet test --configuration Release
```

### Tooling and conventions

This repository uses:

* .NET 10
* Nullable enabled
* Implicit usings enabled
* Central package management
* Microsoft Testing Platform
* xUnit v3
* FluentAssertions
* Nerdbank.GitVersioning

## API / Contracts / Examples

### Core types

* `Error` — immutable error model with `Message`, `Type`, and `Metadata`
* `ErrorType` — supported error categories:

  * `Validation`
  * `NotFound`
  * `Conflict`
  * `Unauthorized`
  * `Forbidden`
  * `Failure`
  * `Unexpected`
* `Result` — success or failure without a value
* `Result<T>` — success or failure with a value

### Core operations

* `Result.Success()`
* `Result.Success<T>(value)`
* `Result.Failure(...)`
* `Result.Combine(...)`
* `Map(...)`
* `Bind(...)`
* `BindAsync(...)`
* `Ensure(...)`
* `Tap(...)`
* `Match(...)`

### Working with errors

Factory methods:

* `Error.Validation(message)`
* `Error.NotFound(message)`
* `Error.Conflict(message)`
* `Error.Unauthorized(message)`
* `Error.Forbidden(message)`
* `Error.Failure(message)`
* `Error.Unexpected(message)`

Additional helpers:

* `WithMetadata(key, value)`
* `WithField(field)`

### Working with values in `Result<T>`

* `Value` — returns the value on success, otherwise throws `InvalidOperationException`
* `ValueOrDefault` — returns the value on success, or `default` on failure
* `TryGetValue(out value)` — safely attempts to get the value

### Working with errors in `Result`

* `Errors` — returns the list of errors
* `FirstError` — returns the first error, otherwise throws `InvalidOperationException`
* `IsSuccess` / `IsFailure` — explicit result state checks

### Behavioral notes

* `Value` throws `InvalidOperationException` when the result is a failure.
* `FirstError` throws `InvalidOperationException` when the result is successful.
* `Combine` aggregates errors from all failed results.
* `Match` is intended for finishing a result pipeline at the application boundary.

## Roadmap / TODO

Potential future improvements:

* add more advanced composition helpers if a clear use case appears;
* extend documentation with more domain-oriented examples;
* add dedicated examples for ASP.NET Core minimal APIs;
* keep the package as a reusable standard for future PANiXiDA NuGet libraries.

## Contributing

Contributions are welcome if they keep the package focused and predictable.

### General rules

* keep the public API small and intentional;
* avoid unnecessary dependencies;
* preserve existing naming;
* do not introduce breaking API changes without a strong reason;
* public APIs must have XML documentation in English.

### Code style

* follow the repository `.editorconfig`;
* do not introduce expression-bodied method declarations;
* prefer explicit and readable code over overly compact code.

### Tests

* add or update tests for every meaningful behavior change;
* cover happy path, guard clauses, and failure scenarios;
* verify public API behavior, not implementation details, unless required;
* add a regression test first when fixing a bug;
* do not add `using Xunit;` or `using FluentAssertions;` in test files, because they are provided as global usings in the test project;
* write `DisplayName` values in English;
* structure tests using the Arrange, Act, Assert pattern.

### Validation before completion

Before considering work complete, run:

```bash
dotnet restore
dotnet format
dotnet build --configuration Release
dotnet test --configuration Release
```

## License

This project is licensed under the Apache License, Version 2.0.

See the [LICENSE](LICENSE) file for details.

## Maintainers / Contacts

Maintained by the PANiXiDA.

Repository:

* `PANiXiDA-Dotnet-Core/result-pattern`

For questions or improvements, use:

* GitHub Issues
* Pull Requests
* repository discussions, if enabled
