using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class Flow
{
    private const int MaxStudent = 25;

    private Schedule _schedule = new Schedule();
    private List<ExtraStudent> _students = new List<ExtraStudent>();

    public Flow(Ognp ognp, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new IsuExtraException("Name is incorrect");

        ArgumentNullException.ThrowIfNull(ognp);

        (Name, Ognp) = (name, ognp);
    }

    public IReadOnlyList<ExtraStudent> Students => _students;
    public string Name { get; }
    public char Faculty => Ognp.FacultyCode;
    public Ognp Ognp { get; }
    public Schedule Schedule => _schedule;

    public void AddStudent(ExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(student);

        if (_students.Contains(student))
            throw new IsuExtraException("There is duplicate of students");

        if (_students.Count >= MaxStudent)
            throw new IsuExtraException("Maximum reached");

        _students.Add(student);
    }

    public void RemoveStudent(ExtraStudent student)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (!_students.Remove(student))
            throw new IsuExtraException("Student not found");
    }
}