using Backups.Exceptions;

namespace Backups.Back;

public class RestorePoint
{
    public RestorePoint(Storage storage, string name)
    {
        ArgumentNullException.ThrowIfNull(storage);
        if (string.IsNullOrWhiteSpace(name))
            throw new BackupsException("Name or path is null or empty");

        Storage = storage;
        Name = name;
    }

    public DateTime CreationTime { get; } = DateTime.Now;

    public Storage Storage { get; }
    public string Name { get; }
}