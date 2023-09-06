using Backups.Algorithms;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Back;

public class BackupTask
{
    private List<BackupObject> _objects;

    public BackupTask(string name, IRepository repository, IStorage storageType, IFolder backupsFolder, Backup backup, IArchiver archiver)
    {
        if (string.IsNullOrEmpty(name))
            throw new BackupsException("Name cannot be null or empty");
        Name = name;
        Repository = repository ?? throw new BackupsException("Repository cannot be null");
        StorageType = storageType ?? throw new BackupsException("Storage model cannot be null");
        BackupsFolder = backupsFolder ?? throw new BackupsException("Backups folder cannot be null");
        Archiver = archiver ?? throw new BackupsException("Archiver cannot be null");
        CurrentBackup = backup ?? throw new BackupsException("Backup cannot be null");
        _objects = new List<BackupObject>();
    }

    public Backup CurrentBackup { get; set; }
    public IStorage StorageType { get; }
    public IRepository Repository { get; }
    public IFolder BackupsFolder { get; set; }
    public string Name { get; }
    public IArchiver Archiver { get; }
    public IReadOnlyList<BackupObject> Objects => _objects;
    public IReadOnlyList<RestorePoint> RestorePoints => CurrentBackup.RestorePoints;

    public void AddObject(IObject backupObject)
    {
        if (backupObject is null)
            throw new BackupsException("Backup object cannot be null");

        if (_objects.Any(o => o.Path == backupObject.Path))
            throw new BackupsException("Backup object already exists");

        _objects.Add(new BackupObject(backupObject.Path, backupObject));
    }

    public void RemoveObject(IObject backupObject)
    {
        if (backupObject is null)
            throw new BackupsException("Backup object cannot be null");

        if (_objects.All(o => o.Path != backupObject.Path))
            throw new BackupsException("Backup object does not exist");

        _objects.Remove(_objects.First(o => o.Path == backupObject.Path));
    }

    public RestorePoint CreateRestorePoint(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new BackupsException("Name cannot be null or empty");
        if (CurrentBackup.RestorePoints.Any(p => p.Name == name))
            throw new BackupsException("Restore point with this name already exists");

        IFolder folder = BackupsFolder.CreateFolder(name);
        Storage storage = StorageType.CreateStorage(Objects, folder, Archiver);
        var restorePoint = new RestorePoint(storage, name);
        CurrentBackup.AddRestorePoint(restorePoint);
        return restorePoint;
    }
}