using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Back;

public class BackupObject
{
    public BackupObject(string name, IObject contents)
    {
        ArgumentNullException.ThrowIfNull(contents);
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Name or path is null or empty");
        Name = name;
        Contents = contents;
    }

    public string Name { get; }
    public IObject Contents { get; }
    public string Path => Contents.Path;
}