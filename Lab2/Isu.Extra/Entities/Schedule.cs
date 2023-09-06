using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class Schedule
{
    private List<Lesson> _lessons = new List<Lesson>();

    public Schedule()
    { }

    public IReadOnlyList<Lesson> Lessons => _lessons;

    public void AddLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);
        if (_lessons.Any(l => l.Intersects(lesson)))
            throw new IsuExtraException("Lesson intersects existing one");
        _lessons.Add(lesson);
    }

    public bool Intersects(Schedule other) => _lessons.Any(l1 => other.Lessons.Any(l2 => l2.Intersects(l1)));
}