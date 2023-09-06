using Backups.Back;
using Backups.Interfaces;

namespace Backups.Algorithms;

public class SplitStorage : IStorage
{
    public Storage CreateStorage(IReadOnlyList<BackupObject> objects, IFolder destination, IArchiver archiver)
    {
        foreach (BackupObject obj in objects)
        {
            IFile file = destination.CreateFile(obj.Name);
            var stream = new MemoryStream();
            archiver.Archive(obj.Contents, stream);
            file.WriteData(stream);
        }

        return new Storage(destination);
    }
}