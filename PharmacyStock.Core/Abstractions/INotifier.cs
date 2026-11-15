namespace PharmacyStock.Core.Abstractions;

/// <summary>
/// Defines patient notification operations.
/// </summary>
public interface INotifier
{
    /// <summary>
    /// Sends a notification to a patient.
    /// </summary>
    /// <param name="recipient">Notification recipient.</param>
    /// <param name="message">Notification message.</param>
    void Notify(string recipient, string message);
}
