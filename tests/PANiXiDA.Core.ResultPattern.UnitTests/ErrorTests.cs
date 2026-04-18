namespace PANiXiDA.Core.ResultPattern.UnitTests;

public sealed class ErrorTests
{
    [Fact(DisplayName = "Error ctor → sets properties when valid message, type, and metadata are provided")]
    public void Constructor_Should_SetProperties_When_MessageTypeAndMetadataAreValid()
    {
        const string message = "Validation failed";
        const string metadataKey = "code";
        const int metadataValue = 42;
        var metadata = new Dictionary<string, object?>
        {
            [metadataKey] = metadataValue
        };

        var error = new Error(message, ErrorType.Validation, metadata);

        error.Message.Should().Be(message);
        error.Type.Should().Be(ErrorType.Validation);
        error.Metadata.Should().ContainSingle()
            .Which.Should().Be(new KeyValuePair<string, object?>(metadataKey, metadataValue));
        error.Metadata.Should().NotBeSameAs(metadata);
    }

    [Fact(DisplayName = "Error ctor → creates a defensive metadata copy when metadata is provided")]
    public void Constructor_Should_CreateDefensiveMetadataCopy_When_MetadataIsProvided()
    {
        const string metadataKey = "code";
        const int originalMetadataValue = 42;
        const int changedMetadataValue = 100;
        var metadata = new Dictionary<string, object?>
        {
            [metadataKey] = originalMetadataValue
        };

        var error = new Error("Validation failed", ErrorType.Validation, metadata);

        metadata[metadataKey] = changedMetadataValue;
        metadata["extra"] = true;

        error.Metadata.Should().ContainSingle()
            .Which.Should().Be(new KeyValuePair<string, object?>(metadataKey, originalMetadataValue));
    }

    [Fact(DisplayName = "Error metadata → throws NotSupportedException when metadata is cast to a mutable dictionary and modified")]
    public void Metadata_Should_ThrowNotSupportedException_When_CastToMutableDictionaryAndModified()
    {
        var error = Error.Validation("Validation failed")
            .WithMetadata("code", 42);
        var metadata = error.Metadata.Should()
            .BeAssignableTo<IDictionary<string, object?>>()
            .Subject;

        Action act = () => metadata["code"] = 100;

        act.Should().Throw<NotSupportedException>();
    }

    [Fact(DisplayName = "Error ctor → creates empty metadata when metadata is not provided")]
    public void Constructor_Should_InitializeEmptyMetadata_When_MetadataIsNull()
    {
        const string message = "Validation failed";

        var error = new Error(message, ErrorType.Validation);

        error.Metadata.Should().BeEmpty();
    }

    [Fact(DisplayName = "Error copy → returns an equivalent new error when copied with record syntax")]
    public void Copy_Should_ReturnEquivalentNewError_When_RecordWithExpressionIsUsed()
    {
        var error = Error.Validation("Validation failed")
            .WithField("email");

        var copiedError = error with { };

        copiedError.Should().NotBeSameAs(error);
        copiedError.Should().Be(error);
    }

    [Fact(DisplayName = "Error ctor → throws ArgumentException when message is null")]
    public void Constructor_Should_ThrowArgumentException_When_MessageIsNull()
    {
        Action act = static () => _ = new Error(null!, ErrorType.Validation);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("message");
    }

    [Fact(DisplayName = "Error ctor → throws ArgumentException when message is empty")]
    public void Constructor_Should_ThrowArgumentException_When_MessageIsEmpty()
    {
        Action act = () => _ = new Error(string.Empty, ErrorType.Validation);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("message");
    }

    [Fact(DisplayName = "Error ctor → throws ArgumentException when message contains only whitespace")]
    public void Constructor_Should_ThrowArgumentException_When_MessageIsWhitespace()
    {
        Action act = () => _ = new Error("   ", ErrorType.Validation);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("message");
    }

    [Fact(DisplayName = "Validation → returns a Validation error when a message is provided")]
    public void Validation_Should_ReturnValidationError_When_MessageIsProvided()
    {
        const string message = "Validation failed";

        var error = Error.Validation(message);

        AssertError(error, message, ErrorType.Validation);
    }

    [Fact(DisplayName = "NotFound → returns a NotFound error when a message is provided")]
    public void NotFound_Should_ReturnNotFoundError_When_MessageIsProvided()
    {
        const string message = "Entity not found";

        var error = Error.NotFound(message);

        AssertError(error, message, ErrorType.NotFound);
    }

    [Fact(DisplayName = "Conflict → returns a Conflict error when a message is provided")]
    public void Conflict_Should_ReturnConflictError_When_MessageIsProvided()
    {
        const string message = "Entity already exists";

        var error = Error.Conflict(message);

        AssertError(error, message, ErrorType.Conflict);
    }

    [Fact(DisplayName = "Unauthorized → returns an Unauthorized error when a message is provided")]
    public void Unauthorized_Should_ReturnUnauthorizedError_When_MessageIsProvided()
    {
        const string message = "Authentication required";

        var error = Error.Unauthorized(message);

        AssertError(error, message, ErrorType.Unauthorized);
    }

    [Fact(DisplayName = "Forbidden → returns a Forbidden error when a message is provided")]
    public void Forbidden_Should_ReturnForbiddenError_When_MessageIsProvided()
    {
        const string message = "Access denied";

        var error = Error.Forbidden(message);

        AssertError(error, message, ErrorType.Forbidden);
    }

    [Fact(DisplayName = "Failure → returns a Failure error when a message is provided")]
    public void Failure_Should_ReturnFailureError_When_MessageIsProvided()
    {
        const string message = "Operation failed";

        var error = Error.Failure(message);

        AssertError(error, message, ErrorType.Failure);
    }

    [Fact(DisplayName = "Unexpected → returns an Unexpected error when a message is provided")]
    public void Unexpected_Should_ReturnUnexpectedError_When_MessageIsProvided()
    {
        const string message = "Unexpected error";

        var error = Error.Unexpected(message);

        AssertError(error, message, ErrorType.Unexpected);
    }

    [Fact(DisplayName = "WithMetadata → adds new metadata when the key does not exist")]
    public void WithMetadata_Should_AddMetadata_When_KeyDoesNotExist()
    {
        const string message = "Validation failed";
        var key = Error.FieldMetadataKey;
        const string value = "name";
        var error = Error.Validation(message);

        var updatedError = error.WithMetadata(key, value);

        updatedError.Metadata.Should().ContainSingle()
            .Which.Should().Be(new KeyValuePair<string, object?>(key, value));
    }

    [Fact(DisplayName = "WithMetadata → overwrites metadata when the key already exists")]
    public void WithMetadata_Should_OverrideMetadata_When_KeyAlreadyExists()
    {
        var key = Error.FieldMetadataKey;
        const string message = "Validation failed";
        const string originalValue = "old-name";
        const string updatedValue = "new-name";
        var error = new Error(
            message,
            ErrorType.Validation,
            new Dictionary<string, object?> { [key] = originalValue });

        var updatedError = error.WithMetadata(key, updatedValue);

        updatedError.Metadata.Should().ContainSingle()
            .Which.Should().Be(new KeyValuePair<string, object?>(key, updatedValue));
    }

    [Fact(DisplayName = "WithMetadata → returns a new error and does not modify the original when metadata is updated")]
    public void WithMetadata_Should_ReturnNewErrorWithoutMutatingOriginal_When_MetadataIsUpdated()
    {
        var key = Error.FieldMetadataKey;
        const string message = "Validation failed";
        const string originalValue = "old-name";
        const string metadataKey = "code";
        const int metadataValue = 100;
        var error = new Error(
            message,
            ErrorType.Validation,
            new Dictionary<string, object?> { [key] = originalValue });

        var updatedError = error.WithMetadata(metadataKey, metadataValue);

        updatedError.Should().NotBeSameAs(error);

        error.Metadata.Should().ContainSingle()
            .Which.Should().Be(new KeyValuePair<string, object?>(key, originalValue));

        updatedError.Metadata.Should().HaveCount(2);
        updatedError.Metadata.Should().Contain(new KeyValuePair<string, object?>(key, originalValue));
        updatedError.Metadata.Should().Contain(new KeyValuePair<string, object?>(metadataKey, metadataValue));
    }

    [Fact(DisplayName = "WithField → adds field metadata when a valid field name is provided")]
    public void WithField_Should_AddFieldMetadata_When_FieldIsValid()
    {
        const string message = "Validation failed";
        const string field = "name";
        var error = Error.Validation(message);

        var updatedError = error.WithField(field);

        updatedError.Metadata.Should().ContainSingle()
            .Which.Should().Be(new KeyValuePair<string, object?>(Error.FieldMetadataKey, field));
    }

    [Fact(DisplayName = "WithField → throws ArgumentException when field is null")]
    public void WithField_Should_ThrowArgumentException_When_FieldIsNull()
    {
        var error = Error.Validation("Validation failed");
        Action act = () => error.WithField(null!);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("field");
    }

    [Fact(DisplayName = "WithField → throws ArgumentException when field is empty")]
    public void WithField_Should_ThrowArgumentException_When_FieldIsEmpty()
    {
        var error = Error.Validation("Validation failed");
        Action act = () => error.WithField(string.Empty);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("field");
    }

    [Fact(DisplayName = "WithField → throws ArgumentException when field contains only whitespace")]
    public void WithField_Should_ThrowArgumentException_When_FieldIsWhitespace()
    {
        var error = Error.Validation("Validation failed");
        Action act = () => error.WithField("   ");

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("field");
    }

    private static void AssertError(Error error, string expectedMessage, ErrorType expectedType)
    {
        error.Message.Should().Be(expectedMessage);
        error.Type.Should().Be(expectedType);
        error.Metadata.Should().BeEmpty();
    }
}
