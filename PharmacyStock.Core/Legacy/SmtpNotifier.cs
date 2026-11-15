namespace PharmacyStock.Core.Legacy;

/// <summary>
/// Simulates a legacy email notification service.
/// </summary>
public sealed class SmtpNotifier
{
    private readonly TextWriter _output;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SmtpNotifier"/> class.
    /// </summary>
    public SmtpNotifier()
    {
        _output = Console.Out;
    }

    /// <summary>
    /// Simulates sending a message.
    /// </summary>
    /// <param name="to">Recipient email.</param>
    /// <param name="subject">Message subject.</param>
    /// <param name="body">Message body.</param>
    public void Send(string to, string subject, string body)
    {
        _output.WriteLine(subject + " для " + to + ": " + body);
    }
}
