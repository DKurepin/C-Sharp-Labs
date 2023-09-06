using Backups.Back;

namespace Backups.Interfaces;

public interface IStorage
{
    Storage CreateStorage(IReadOnlyList<BackupObject> objects, IFolder destination, IArchiver archiver);
}