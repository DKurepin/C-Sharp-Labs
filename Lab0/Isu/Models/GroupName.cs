using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    public const int MinNumber = 1000;

    public GroupName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new IsuException("The name is empty");

        if (name[0] < 'A' || name[0] > 'Z')
            throw new IsuException("There is no letter code for the faculty");

        FacultyCode = name[0];

        if (!int.TryParse(name.Substring(1), out int groupNumber))
            throw new IsuException("Invalid number");

        if (groupNumber < MinNumber)
            throw new IsuException("Number is less than a minimum");

        GroupNumber = groupNumber;
    }

    public char FacultyCode { get; }
    public int GroupNumber { get; }

    public int Course
    {
        get
        {
            double pow = Math.Log10(GroupNumber) - 1;
            int result = (int)(GroupNumber / Math.Pow(10, pow)) % 10;
            return result;
        }
    }
}