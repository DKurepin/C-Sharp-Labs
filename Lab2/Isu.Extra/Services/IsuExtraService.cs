using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Tools;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Services;

public class IsuExtraService
{
    private IsuService _isu;
    private List<ExtraStudent> _extraStudents;
    private List<ExtraGroup> _extraGroups;
    private List<Ognp> _ognps;

    public IsuExtraService()
    {
        _isu = new IsuService();
        _extraStudents = new List<ExtraStudent>();
        _extraGroups = new List<ExtraGroup>();
        _ognps = new List<Ognp>();
    }

    public Ognp AddOgnp(string name)
    {
        if (_ognps.Any(ognp => ognp.Name == name))
            throw new IsuExtraException("This name already exists");
        var newOgnp = new Ognp(name);
        _ognps.Add(newOgnp);
        return newOgnp;
    }

    public bool OgnpHasAnyFlow(Flow flow)
    {
        ArgumentNullException.ThrowIfNull(flow);
        return _ognps.Any(ognp => ognp.Flows.Any(f => f == flow));
    }

    public bool OgnpHasFlow(Flow flow)
    {
        return _ognps.Any(ognp => ognp.Flows.Contains(flow));
    }

    public void SubscribeStudentToFlow(Flow flow, ExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(flow);
        ArgumentNullException.ThrowIfNull(student);

        if (!OgnpHasAnyFlow(flow))
            throw new IsuExtraException("Flow has not found");

        if (!_extraStudents.Contains(student))
            throw new IsuExtraException("Student has not found");

        if (student.ExtraGroup.Schedule.Intersects(flow.Schedule))
            throw new IsuExtraException("There is intersect in schedule");

        student.AddFlow(flow);
        flow.AddStudent(student);
    }

    public void RemoveStudentFromFlow(Flow flow, ExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(flow);
        ArgumentNullException.ThrowIfNull(student);

        if (!OgnpHasAnyFlow(flow))
            throw new IsuExtraException("Flow has not found");

        if (!_extraStudents.Contains(student))
            throw new IsuExtraException("Student not found");

        flow.RemoveStudent(student);
    }

    public IReadOnlyList<Flow> GetFlowsForOgnp(Ognp ognp)
    {
        ArgumentNullException.ThrowIfNull(ognp);

        if (!_ognps.Contains(ognp))
            throw new IsuExtraException("OGNP has not found");

        return ognp.Flows;
    }

    public IReadOnlyList<ExtraStudent> GetStudentsForFlow(Flow flow)
    {
        ArgumentNullException.ThrowIfNull(flow);

        if (!OgnpHasFlow(flow))
            throw new IsuExtraException("Flow has not found");

        return flow.Students;
    }

    public IReadOnlyList<ExtraStudent> GetStudentsWithoutOgnp()
    {
        return _extraStudents
            .Where(student => student.OgnpFlows.Count == 0)
            .ToList();
    }

    public ExtraGroup AddGroup(GroupName name)
    {
        Group group = _isu.AddGroup(name);
        var extraGroup = new ExtraGroup(group);
        _extraGroups.Add(extraGroup);
        return extraGroup;
    }

    public ExtraStudent AddStudent(ExtraGroup extraGroup, string name)
    {
        Group? group = _isu.FindGroup(extraGroup.GroupName);

        if (group is null)
            throw new IsuExtraException("Group has not found");

        Student student = _isu.AddStudent(group!, name);
        var extraStudent = new ExtraStudent(student, extraGroup);
        _extraStudents.Add(extraStudent);
        return extraStudent;
    }

    public Flow AddFlow(Ognp ognp, string name)
    {
        ArgumentNullException.ThrowIfNull(ognp);

        if (!_ognps.Contains(ognp))
            throw new IsuExtraException("OGNP has not found");

        return ognp.AddFlow(name);
    }

    public ExtraStudent GetStudent(int id)
    {
        ExtraStudent? result = FindStudent(id);
        if (result is null)
            throw new IsuExtraException("Student has not found");
        return result!;
    }

    public ExtraStudent? FindStudent(int id) => _extraStudents.Find(student => student.Id == id);

    public IReadOnlyList<ExtraStudent> FindStudents(GroupName groupName)
    {
        return _extraGroups
            .Where(group => group.GroupName == groupName)
            .SelectMany(group => group.ExtraStudents)
            .ToList();
    }

    public IReadOnlyList<ExtraStudent> FindStudents(CourseNumber courseNumber)
    {
        return _extraGroups
            .Where(group => group.GroupName.Course == courseNumber.Value)
            .SelectMany(group => group.ExtraStudents)
            .ToList();
    }

    public ExtraGroup? FindGroup(GroupName groupName) => _extraGroups.Find(group => group.GroupName == groupName);

    public IReadOnlyList<ExtraGroup> FindGroups(CourseNumber courseNumber)
    {
        return _extraGroups
            .Where(group => group.GroupName.Course == courseNumber.Value)
            .ToList();
    }

    public void ChangeStudentGroup(ExtraStudent extraStudent, ExtraGroup newExtraGroup)
    {
        ArgumentNullException.ThrowIfNull(extraStudent);
        ArgumentNullException.ThrowIfNull(newExtraGroup);

        Group? newGroup = FindGroup(newExtraGroup.GroupName);
        if (newGroup is null)
            throw new IsuExtraException("Group has not found");

        Student? student = FindStudent(extraStudent.Id);
        if (student is null)
            throw new IsuExtraException("Student has not found");

        _isu.ChangeStudentGroup(student!, newGroup!);
        extraStudent.ExtraGroup.RemoveExtraStudent(extraStudent);
        extraStudent.ChangeExtraGroup(newExtraGroup);
    }
}