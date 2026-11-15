namespace PharmacyStock.Core.Reporting;

/// <summary>
/// Writes a prescription report to an output stream.
/// </summary>
public sealed class DocumentReportPrinter
{
    private readonly TextWriter _output;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DocumentReportPrinter"/> class.
    /// </summary>
    /// <param name="output">Output text writer.</param>
    public DocumentReportPrinter(TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(output);
        _output = output;
    }

    /// <summary>
    /// Writes the report to the output.
    /// </summary>
    /// <param name="report">Report text.</param>
    public void Print(string report)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(report);
        _output.Write(report);
    }
}
