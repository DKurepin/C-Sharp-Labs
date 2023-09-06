using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class Lesson
{
    public Lesson(TimeOnly start, TimeOnly end, string name, string day)
    {
        if (string.IsNullOrWhiteSpace(day))
            throw new IsuExtraException("Day should be in letters");

        if (string.IsNullOrWhiteSpace(name))
            throw new IsuExtraException("Name of the lesson is incorrect");

        if (start > end)
            throw new IsuExtraException("Invalid time");

        (Name, TimeStart, TimeEnd, Day) = (name, start, end, day);
    }

    public TimeOnly TimeEnd { get; }

    public TimeOnly TimeStart { get; }

    public string Name { get; }

    public string Day { get; }

    public bool Intersects(Lesson other)
    {
        return (TimeStart < other.TimeStart && other.TimeStart < TimeEnd && Day == other.Day)
               || (TimeStart < other.TimeEnd && other.TimeEnd < TimeEnd && Day == other.Day);
    }
}