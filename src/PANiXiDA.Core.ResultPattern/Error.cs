using System.Collections.ObjectModel;

namespace PANiXiDA.Core.ResultPattern;

/// <summary>
/// Represents an immutable error returned by an operation result.
/// </summary>
public sealed record Error
{
    /// <summary>
    /// The metadata key used to store the field name for validation errors.
    /// </summary>
    public const string FieldMetadataKey = "field";

    /// <summary>
    /// Initializes a new instance of <see cref="Error"/>.
    /// </summary>
    /// <param name="message">A human-readable error message.</param>
    /// <param name="type">The error category.</param>
    /// <param name="metadata">Additional machine-readable error data.</param>
    public Error(
        string message,
        ErrorType type,
        IReadOnlyDictionary<string, object?>? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Error message cannot be empty.", nameof(message));
        }

        Message = message;
        Type = type;
        Metadata = new ReadOnlyDictionary<string, object?>(
            metadata is null
                ? []
                : new Dictionary<string, object?>(metadata));
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the error category.
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Gets additional machine-readable error details.
    /// </summary>
    public IReadOnlyDictionary<string, object?> Metadata { get; }

    /// <summary>
    /// Creates a validation error.
    /// </summary>
    /// <param name="message">The validation error message.</param>
    /// <returns>A new instance of <see cref="Error"/> with type <see cref="ErrorType.Validation"/>.</returns>
    public static Error Validation(string message)
    {
        return new Error(message, ErrorType.Validation);
    }

    /// <summary>
    /// Creates an error for a missing entity or resource.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new instance of <see cref="Error"/> with type <see cref="ErrorType.NotFound"/>.</returns>
    public static Error NotFound(string message)
    {
        return new Error(message, ErrorType.NotFound);
    }

    /// <summary>
    /// Creates a state conflict error.
    /// </summary>
    /// <param name="message">The conflict error message.</param>
    /// <returns>A new instance of <see cref="Error"/> with type <see cref="ErrorType.Conflict"/>.</returns>
    public static Error Conflict(string message)
    {
        return new Error(message, ErrorType.Conflict);
    }

    /// <summary>
    /// Creates an authorization error.
    /// </summary>
    /// <param name="message">The authorization error message.</param>
    /// <returns>A new instance of <see cref="Error"/> with type <see cref="ErrorType.Unauthorized"/>.</returns>
    public static Error Unauthorized(string message)
    {
        return new Error(message, ErrorType.Unauthorized);
    }

    /// <summary>
    /// Creates an insufficient permissions error.
    /// </summary>
    /// <param name="message">The access error message.</param>
    /// <returns>A new instance of <see cref="Error"/> with type <see cref="ErrorType.Forbidden"/>.</returns>
    public static Error Forbidden(string message)
    {
        return new Error(message, ErrorType.Forbidden);
    }

    /// <summary>
    /// Creates a business or application failure error.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new instance of <see cref="Error"/> with type <see cref="ErrorType.Failure"/>.</returns>
    public static Error Failure(string message)
    {
        return new Error(message, ErrorType.Failure);
    }

    /// <summary>
    /// Creates an unexpected error.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new instance of <see cref="Error"/> with type <see cref="ErrorType.Unexpected"/>.</returns>
    public static Error Unexpected(string message)
    {
        return new Error(message, ErrorType.Unexpected);
    }

    /// <summary>
    /// Returns a copy of the current error with an added or replaced metadata entry.
    /// </summary>
    /// <param name="key">The metadata key to add or replace.</param>
    /// <param name="value">The metadata value associated with the key.</param>
    /// <returns>A new instance of <see cref="Error"/> with updated metadata.</returns>
    public Error WithMetadata(string key, object? value)
    {
        var metadata = new Dictionary<string, object?>(Metadata)
        {
            [key] = value
        };

        return new Error(Message, Type, metadata);
    }

    /// <summary>
    /// Returns a copy of the current error with the field name stored in metadata.
    /// </summary>
    /// <param name="field">The field name associated with the error.</param>
    /// <returns>A new instance of <see cref="Error"/> with the field metadata populated.</returns>
    public Error WithField(string field)
    {
        if (string.IsNullOrWhiteSpace(field))
        {
            throw new ArgumentException("Field cannot be empty.", nameof(field));
        }

        return WithMetadata(FieldMetadataKey, field);
    }
}
