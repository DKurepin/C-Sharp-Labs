using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Repositories;

public class InMemoryFile : IFile, IDisposable
{
    private Stream _stream;

    public InMemoryFile(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new BackupsException("File name cannot be null or empty");
        if (path.Contains(System.IO.Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid file name");

        Path = path;
        _stream = new MemoryStream();
    }

    public string Path { get; }

    public void ReadData(Stream output)
    {
        ArgumentNullException.ThrowIfNull(output, nameof(output));
        _stream.CopyTo(output);
    }

    public void WriteData(Stream input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        _stream.SetLength(0);
        input.CopyTo(_stream);
    }

    public void AddData(Stream input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        input.CopyTo(_stream);
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}