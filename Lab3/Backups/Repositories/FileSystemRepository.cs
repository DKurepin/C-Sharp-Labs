using System.IO.Compression;
using Backups.Back;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Repositories;

public class FileSystemRepository : IRepository
{
    public FileSystemRepository(string rootPath)
    {
        Directory.CreateDirectory(rootPath);
        RootFolder = new FileSystemFolder(rootPath);
    }

    public IFolder RootFolder { get; }

    public IObject GetObject(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new BackupsException("Object name cannot be null or empty");

        if (Directory.Exists(path))
            return new FileSystemFolder(path);
        if (File.Exists(path))
            return new FileSystemFile(path);
        throw new BackupsException("Object does not exist");
    }
}