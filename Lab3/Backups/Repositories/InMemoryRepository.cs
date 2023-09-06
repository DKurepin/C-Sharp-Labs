using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Repositories;

public class InMemoryRepository : IRepository
{
    public InMemoryRepository(string rootName)
    {
        if (string.IsNullOrWhiteSpace(rootName))
            throw new BackupsException("Value cannot be null or whitespace.");
        if (rootName.Contains(Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid root path");

        RootFolder = new InMemoryFolder(rootName);
    }

    public IFolder RootFolder { get; }

    public IObject GetObject(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new BackupsException("Object name cannot be null or empty");
        if (path.Contains(Path.DirectorySeparatorChar))
            throw new BackupsException("Invalid path");

        var pathComponents = path.Split(Path.PathSeparator);
        int currPathIndex = 0;
        foreach (var rootComponent in RootFolder.Path.Split(Path.PathSeparator))
        {
            if (rootComponent != pathComponents[currPathIndex])
                throw new BackupsException("Not found");
            currPathIndex++;
        }

        IFolder currFolder = RootFolder;
        while (currPathIndex < pathComponents.Length)
        {
            IObject obj = currFolder.GetObject(pathComponents[currPathIndex]);
            if (obj is IFolder folder)
            {
                currFolder = folder;
            }
            else
            {
                if (currPathIndex + 1 != pathComponents.Length)
                    throw new BackupsException("Not found");
                return obj;
            }
        }

        return currFolder;
    }
}