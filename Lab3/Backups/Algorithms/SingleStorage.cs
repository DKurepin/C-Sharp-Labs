using Backups.Back;
using Backups.Interfaces;

namespace Backups.Algorithms;

public class SingleStorage : IStorage
{
    public Storage CreateStorage(IReadOnlyList<BackupObject> objects, IFolder destination, IArchiver archiver)
    {
        IFolder tempFolder = destination.CreateFolder(".tmp");
        foreach (BackupObject obj in objects)
        {
            IFile file = tempFolder.CreateFile(obj.Name);
            var stream = new MemoryStream();
            archiver.Archive(obj.Contents, stream);
            file.WriteData(stream);
        }

        IFile storage = destination.CreateFile("storage");
        var storageStream = new MemoryStream();
        archiver.Archive(tempFolder, storageStream);
        storage.WriteData(storageStream);
        destination.DeleteObject(".tmp");

        return new Storage(destination);
    }
}