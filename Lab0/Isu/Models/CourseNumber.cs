using Isu.Exceptions;

namespace Isu.Models;

public class CourseNumber
{
    public const int MinValue = 1;
    public const int MaxValue = 7;

    public CourseNumber(int value)
    {
        if (value < MinValue || value > MaxValue)
            throw new IsuException($"value {value} is not allowed");
        Value = value;
    }

    public CourseNumber(GroupName name)
        : this(name.Course)
    {
    }

    public int Value { get; }

    public override string ToString()
    {
        return $"{Value}";
    }
}