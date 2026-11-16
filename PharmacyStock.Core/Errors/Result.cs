namespace PharmacyStock.Core.Errors;

/// <summary>
/// Represents a successful result or a validation error.
/// </summary>
/// <typeparam name="T">Type of the result value.</typeparam>
public readonly record struct Result<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> struct.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the result is successful.</param>
    /// <param name="value">Result value.</param>
    /// <param name="error">Error message.</param>
    internal Result(bool isSuccess, T? value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>Gets the result value.</summary>
    public T? Value { get; }

    /// <summary>Gets the error message.</summary>
    public string Error { get; }
}
