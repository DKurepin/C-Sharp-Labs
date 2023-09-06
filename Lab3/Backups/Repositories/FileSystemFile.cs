using Backups.Interfaces;

namespace Backups.Repositories;

public class FileSystemFile : IFile
{
    public FileSystemFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new FileNotFoundException("Path is null or empty", path);
        if (!File.Exists(path))
            throw new FileNotFoundException("File not found", path);

        Path = path;
    }

    public string Path { get; }

    public void WriteData(Stream input)
    {
        var fs = File.OpenWrite(Path);
        input.CopyTo(input);
        fs.Close();
    }

    public void AddData(Stream input)
    {
        var fs = File.Open(Path, FileMode.Append);
        input.CopyTo(input);
        fs.Close();
    }

    public void ReadData(Stream output)
    {
        var fs = File.Open(Path, FileMode.Open);
        fs.CopyTo(output);
        fs.Close();
    }
}