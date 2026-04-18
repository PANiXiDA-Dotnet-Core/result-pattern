namespace PANiXiDA.Core.ResultPattern;

/// <summary>
/// Defines the error categories supported by the library.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// An input validation error.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// An error indicating that an entity or resource was not found.
    /// </summary>
    NotFound = 2,

    /// <summary>
    /// An error indicating a conflict with the current state.
    /// </summary>
    Conflict = 3,

    /// <summary>
    /// An authorization error.
    /// </summary>
    Unauthorized = 4,

    /// <summary>
    /// An insufficient permissions error.
    /// </summary>
    Forbidden = 5,

    /// <summary>
    /// An expected business or application failure.
    /// </summary>
    Failure = 6,

    /// <summary>
    /// An unexpected error that does not belong to a narrower category.
    /// </summary>
    Unexpected = 7
}
