namespace Backups.Interfaces;

public interface IFile : IObject
{
    void WriteData(Stream input);
    void AddData(Stream input);
    void ReadData(Stream output);
}