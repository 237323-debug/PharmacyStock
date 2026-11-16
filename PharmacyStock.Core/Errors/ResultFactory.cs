namespace PharmacyStock.Core.Errors;

/// <summary>
/// Creates successful and failed operation results.
/// </summary>
public static class ResultFactory
{
    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <typeparam name="T">Type of the result value.</typeparam>
    /// <param name="value">Result value.</param>
    /// <returns>A successful result.</returns>
    public static Result<T> Ok<T>(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Result<T>(true, value, string.Empty);
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <typeparam name="T">Type of the result value.</typeparam>
    /// <param name="error">Error message.</param>
    /// <returns>A failed result.</returns>
    public static Result<T> Fail<T>(string error)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(error);

        return new Result<T>(false, default, error);
    }
}
