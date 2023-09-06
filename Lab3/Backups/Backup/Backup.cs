using Backups.Exceptions;

namespace Backups.Back;

public class Backup
{
    private List<RestorePoint> restorePoints;

    public Backup()
    {
        restorePoints = new List<RestorePoint>();
    }

    public IReadOnlyList<RestorePoint> RestorePoints => restorePoints;

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        if (IsRestorePointExist(restorePoint))
            throw new BackupsException("Restore point already exists");
        restorePoints.Add(restorePoint);
    }

    public bool IsRestorePointExist(RestorePoint restorePoint)
    {
        return restorePoints.Contains(restorePoint);
    }

    public void DeleteRestorePoint(RestorePoint restorePoint)
    {
        if (!restorePoints.Remove(restorePoint))
            throw new BackupsException("Restore point doesn't exist");
    }
}