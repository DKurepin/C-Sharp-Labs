using Backups.Back;

namespace Backups.Interfaces;

public interface IRepository
{
    public IFolder RootFolder { get; }
    IObject GetObject(string path);
}