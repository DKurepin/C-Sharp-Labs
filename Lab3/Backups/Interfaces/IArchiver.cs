namespace Backups.Interfaces;

public interface IArchiver
{
    void Archive(IObject target, Stream output);
}