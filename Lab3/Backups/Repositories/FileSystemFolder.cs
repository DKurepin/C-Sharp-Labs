using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Repositories;

public class FileSystemFolder : IFolder
{
    public FileSystemFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new BackupsException("Value cannot be null or whitespace.");
        if (path.Contains(System.IO.Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid path");

        Path = path;
    }

    public string Path { get; }

    public IFile CreateFile(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Value cannot be null or whitespace.");
        if (name.Contains(System.IO.Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid name");

        var path = System.IO.Path.Join(Path, name);
        File.Create(path).Dispose();
        return new FileSystemFile(path);
    }

    public IFolder CreateFolder(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Value cannot be null or whitespace.");
        if (name.Contains(System.IO.Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid name");

        var path = System.IO.Path.Join(Path, name);
        File.Create(path).Dispose();
        return new FileSystemFolder(path);
    }

    public void DeleteObject(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Value cannot be null or whitespace.");
        if (name.Contains(System.IO.Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid name");

        var path = System.IO.Path.Join(Path, name);
        Directory.Delete(name);
    }

    public IObject GetObject(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Value cannot be null or whitespace.");
        if (name.Contains(System.IO.Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid name");

        var path = System.IO.Path.Join(Path, name);
        if (File.Exists(path))
            return new FileSystemFile(path);
        if (Directory.Exists(path))
            return new FileSystemFolder(path);
        throw new BackupsException("Not found");
    }

    public IReadOnlyList<IObject> Contents()
    {
        var files = Directory.GetFiles(Path).Select(p => new FileSystemFile(p));
        var directories = Directory.GetDirectories(Path).Select(p => new FileSystemFile(p));
        var result = new List<IObject>();
        result.AddRange(files);
        result.AddRange(directories);
        return result;
    }
}