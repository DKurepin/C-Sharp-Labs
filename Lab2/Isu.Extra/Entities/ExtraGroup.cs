using Isu.Entities;
using Isu.Extra.Tools;
using Isu.Models;

namespace Isu.Extra.Entities;

public class ExtraGroup : Group
{
    private Schedule _schedule = new Schedule();
    private List<ExtraStudent> _extraStudents = new List<ExtraStudent>();

    public ExtraGroup(GroupName groupName)
        : base(groupName)
    {
    }

    public ExtraGroup(Group group)
        : base(group.GroupName) { }

    public IReadOnlyList<ExtraStudent> ExtraStudents => _extraStudents;
    public Schedule Schedule => _schedule;

    public void AddExtraStudent(ExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (_extraStudents.Count >= MaxStudents)
            throw new IsuExtraException("The group is full");
        _extraStudents.Add(student);
    }

    public void RemoveExtraStudent(ExtraStudent student)
    {
        if (!_extraStudents.Remove(student))
            throw new IsuExtraException("There is no such student");
    }
}