using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Algorithms;

public class ZipArchiver : IArchiver
{
    public void Archive(IObject target, Stream output)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(output);

        using var archive = new ZipArchive(output, ZipArchiveMode.Create, true);
        if (target is IFolder folder)
            ArchiveFolder(folder, archive, target.Path.Length);
        else if (target is IFile file)
            ArchiveFile(file, archive, Path.GetDirectoryName(file.Path) !.Length + 1);
    }

    private void ArchiveFolder(IFolder target, ZipArchive archive, int relativePathStart)
    {
        foreach (IObject obj in target.Contents())
        {
            if (obj is IFolder folder)
                ArchiveFolder(folder, archive, relativePathStart);
            else if (obj is IFile file)
                ArchiveFile(file, archive, relativePathStart);
        }
    }

    private void ArchiveFile(IFile target, ZipArchive archive, int relativePathStart)
    {
        ZipArchiveEntry archiveEntry = archive.CreateEntry(target.Path.Substring(relativePathStart));
        using Stream archiveStream = archiveEntry.Open();
        target.ReadData(archiveStream);
    }
}