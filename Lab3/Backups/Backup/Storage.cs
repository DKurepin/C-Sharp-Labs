using Backups.Interfaces;

namespace Backups.Back;

public class Storage
{
    public Storage(IFolder location)
    {
        ArgumentNullException.ThrowIfNull(location);
        Location = location;
    }

    public IFolder Location { get; }
}