using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private int _nextId = 300000;
    private List<Student> _students = new List<Student>();
    private List<Group> _groups = new List<Group>();

    public IsuService()
    {
    }

    public Group AddGroup(GroupName name)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (HasGroupName(name))
            throw new IsuException("Duplicate of group");

        var newGroup = new Group(name);
        _groups.Add(newGroup);
        return newGroup;
    }

    public Student AddStudent(Group group, string name)
    {
        ArgumentNullException.ThrowIfNull(group);

        if (string.IsNullOrWhiteSpace(name))
            throw new IsuException("The name is empty");

        if (!HasGroup(group))
            throw new IsuException("The group has not found");

        var newStudent = new Student(_nextId, name, group);
        group.AddStudent(newStudent);
        _students.Add(newStudent);
        _nextId++;
        return newStudent;
    }

    public Student GetStudent(int id)
    {
        Student? student = FindStudent(id);
        if (student is null)
            throw new IsuException("The student has not found");
        return student!;
    }

    public Student? FindStudent(int id) => _students.Find(s => s.Id == id);

    public IReadOnlyList<Student> FindStudents(GroupName groupName)
    {
        ArgumentNullException.ThrowIfNull(groupName);

        Group? group = FindGroup(groupName);
        if (groupName is null)
            throw new IsuException("The group has not found");
        return group!.Students;
    }

    public IReadOnlyList<Student> FindStudents(CourseNumber courseNumber)
    {
        ArgumentNullException.ThrowIfNull(courseNumber);

        List<Student> result =
            _groups
                .Where(g => g.GroupName.Course == courseNumber.Value)
                .SelectMany(g => g.Students)
                .ToList();
        return result;
    }

    public Group? FindGroup(GroupName groupName) => _groups.Find(g => g.GroupName == groupName);

    public IReadOnlyList<Group> FindGroups(CourseNumber courseNumber)
    {
        ArgumentNullException.ThrowIfNull(courseNumber);

        List<Group> result =
            _groups
                .Where(g => g.GroupName.Course == courseNumber.Value)
                .ToList();
        return result;
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(newGroup);

        if (student.Group == newGroup)
            throw new IsuException("The student is already there");

        if (!_students.Contains(student))
            throw new IsuException("The student has not found");

        if (!_groups.Contains(newGroup))
            throw new IsuException("The group has not found");

        newGroup.AddStudent(student);
        student.Group.RemoveStudent(student);
        student.ChangeGroup(newGroup);
    }

    private bool HasGroupName(GroupName name) => _groups.Any(g => g.GroupName == name);

    private bool HasGroup(Group group) => _groups.Any(g => g == group);
}