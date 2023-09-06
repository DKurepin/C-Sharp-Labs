using Isu.Exceptions;

namespace Isu.Entities;

public class Student
{
    public Student(int id, string name, Group group)
    {
        ArgumentNullException.ThrowIfNull(group);

        if (string.IsNullOrWhiteSpace(name))
            throw new IsuException("The name is empty");

        (Id, Name, Group) = (id, name, group);
    }

    public int Id { get; }
    public string Name { get; }
    public Group Group { get; private set; }

    public void ChangeGroup(Group newGroup)
    {
        ArgumentNullException.ThrowIfNull(newGroup);
        Group = newGroup;
    }
}