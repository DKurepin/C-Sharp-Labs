using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    public const int MaxStudents = 30;
    private List<Student> _students = new List<Student>();

    public Group(GroupName groupName)
    {
        ArgumentNullException.ThrowIfNull(groupName);
        GroupName = groupName;
    }

    public IReadOnlyList<Student> Students => _students;
    public GroupName GroupName { get; }

    public void AddStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        if (HasStudent(student))
            throw new IsuException("Already contains student");

        if (_students.Count == MaxStudents)
            throw new IsuException("Already full");

        _students.Add(student);
    }

    public void RemoveStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (!_students.Remove(student))
            throw new IsuException("The student has not found");
    }

    private bool HasStudent(Student student) => _students.Contains(student);
}