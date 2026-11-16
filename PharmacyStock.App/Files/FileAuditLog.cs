using System.Globalization;
using System.Text;

namespace PharmacyStock.App.Files;

/// <summary>
/// Writes pharmacy operations to an audit file.
/// </summary>
public sealed class FileAuditLog : IDisposable
{
    private readonly StreamWriter _writer;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="FileAuditLog"/> class.
    /// </summary>
    /// <param name="path">Path to the audit file.</param>
    public FileAuditLog(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        string fullPath = Path.GetFullPath(path);
        string? directory = Path.GetDirectoryName(fullPath);

        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        _writer = new StreamWriter(
            fullPath,
            append: true,
            encoding: new UTF8Encoding(false));
    }

    /// <summary>
    /// Writes an operation to the audit file.
    /// </summary>
    /// <param name="operation">Operation name.</param>
    /// <param name="documentId">Prescription identifier.</param>
    public void Write(string operation, int documentId)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentException.ThrowIfNullOrWhiteSpace(operation);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(documentId);

        string line = string.Format(
            CultureInfo.InvariantCulture,
            "{0:O};{1};{2}",
            DateTimeOffset.Now,
            operation,
            documentId);

        _writer.WriteLine(line);
    }

    /// <summary>
    /// Releases the audit file.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _writer.Dispose();

        GC.SuppressFinalize(this);
    }
}
