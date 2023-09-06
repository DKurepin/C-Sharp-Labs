using Isu.Entities;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class ExtraStudent : Student
{
    private const int MaxOgnp = 2;
    private List<Flow> _ognpFlows = new List<Flow>();

    public ExtraStudent(int id, string name, Group group, ExtraGroup extraGroup)
        : base(id, name, group)
    {
        ArgumentNullException.ThrowIfNull(extraGroup);
        ExtraGroup = extraGroup;
    }

    public ExtraStudent(Student student, ExtraGroup extraGroup)
        : base(student.Id, student.Name, student.Group)
    {
        ArgumentNullException.ThrowIfNull(extraGroup);
        ExtraGroup = extraGroup;
    }

    public IReadOnlyList<Flow> OgnpFlows => _ognpFlows;
    public ExtraGroup ExtraGroup { get; protected set; }

    public void AddFlow(Flow flow)
    {
        ArgumentNullException.ThrowIfNull(flow);

        if (_ognpFlows.Any(o => o.Faculty == flow.Faculty))
            throw new IsuExtraException("Faculty already exists");

        if (ExtraGroup.GroupName.FacultyCode == flow.Faculty)
            throw new IsuExtraException("Faculty name is not unique");

        if (_ognpFlows.Count >= MaxOgnp)
            throw new IsuExtraException("You reached maximum of OGNP`s");

        _ognpFlows.Add(flow);
    }

    public void RemoveFlow(Flow flow)
    {
        if (!_ognpFlows.Remove(flow))
            throw new IsuExtraException("There is no such flow");
    }

    public void ChangeExtraGroup(ExtraGroup group)
    {
        ArgumentNullException.ThrowIfNull(group);
        ExtraGroup = group;
    }
}