using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Repositories;

public class InMemoryFolder : IFolder
{
    private List<IObject> _contents = new List<IObject>();

    public InMemoryFolder(string path)
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

        var file = new InMemoryFile(name);
        _contents.Add(file);
        return file;
    }

    public IFolder CreateFolder(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Value cannot be null or whitespace.");
        if (name.Contains(System.IO.Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid name");

        var folder = new InMemoryFolder(name);
        _contents.Add(folder);
        return folder;
    }

    public void DeleteObject(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Value cannot be null or whitespace.");
        if (name.Contains(System.IO.Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid name");

        var obj = _contents.FirstOrDefault(o => o.Path == name);
        if (obj == null)
            throw new BackupsException($"Object with name '{name}' not found.");
        _contents.Remove(obj);
    }

    public IObject GetObject(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Value cannot be null or whitespace.");
        if (name.Contains(System.IO.Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid name");

        return _contents.FirstOrDefault(o => o.Path == name) ?? throw new InvalidOperationException();
    }

    public IReadOnlyList<IObject> Contents() => _contents;
}