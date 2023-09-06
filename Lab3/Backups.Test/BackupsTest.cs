using Backups.Algorithms;
using Backups.Back;
using Backups.Interfaces;
using Backups.Repositories;
using Xunit;

namespace Backups.Test;

public class BackupsTest
{
    [Fact]
    public void CreateBackupTask_CheckRestorePointAndStorage()
    {
        var repo = new InMemoryRepository("root");
        IFolder backupDir = repo.RootFolder.CreateFolder("backup");
        var archiver = new ZipArchiver();
        var storageType = new SplitStorage();
        var backup = new Backup();
        var backupTask = new BackupTask("first", repo, storageType, backupDir, backup, archiver);
        IFolder sourceDir = repo.RootFolder.CreateFolder("source");

        IFile fileA = sourceDir.CreateFile("A.txt");
        fileA.WriteData(Input("This text is written in A.txt"));
        backupTask.AddObject(fileA);
        IFile fileB = sourceDir.CreateFile("B.txt");
        fileB.WriteData(Input("This text is written in A.txt"));
        backupTask.AddObject(fileB);
        backupTask.CreateRestorePoint("restorePoint1");
        int restorePointsCount = backupTask.RestorePoints.Count;
        var rpFolder = (backupDir.GetObject("restorePoint1") as IFolder) !;
        int restorePoint1FilesCount = rpFolder.Contents().Count;
        backupTask.RemoveObject(fileA);
        backupTask.CreateRestorePoint("restorePoint2");
        rpFolder = (backupDir.GetObject("restorePoint2") as IFolder) !;
        int restorePoint2FilesCount = rpFolder.Contents().Count;

        Assert.Equal(1, restorePointsCount);
        Assert.Equal(2, restorePoint1FilesCount);
        Assert.Equal(1, restorePoint2FilesCount);
        Assert.Equal(2, backupTask.CurrentBackup.RestorePoints.Count);
    }

    private Stream Input(string input)
    {
        var stream = new MemoryStream();
        var streamWriter = new StreamWriter(stream);
        streamWriter.Write(input);
        stream.Position = 0;
        return stream;
    }
}