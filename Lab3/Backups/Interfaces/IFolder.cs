namespace Backups.Interfaces;

public interface IFolder : IObject
{
    IFile CreateFile(string name);
    IFolder CreateFolder(string name);
    void DeleteObject(string name);
    IObject GetObject(string name);
    IReadOnlyList<IObject> Contents();
}