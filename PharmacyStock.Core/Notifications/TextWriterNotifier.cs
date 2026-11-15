using PharmacyStock.Core.Abstractions;

namespace PharmacyStock.Core.Notifications;

/// <summary>
/// Sends notifications to a text writer.
/// </summary>
public sealed class TextWriterNotifier : INotifier
{
    private readonly TextWriter _output;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TextWriterNotifier"/> class.
    /// </summary>
    /// <param name="output">Output writer.</param>
    public TextWriterNotifier(TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(output);
        _output = output;
    }

    /// <summary>
    /// Writes a notification.
    /// </summary>
    /// <param name="recipient">Recipient.</param>
    /// <param name="message">Message.</param>
    public void Notify(string recipient, string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipient);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        _output.WriteLine(
            "Повідомлення для " + recipient + ": " + message);
    }
}
