using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    private IsuService isu = new IsuService();

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var name = new GroupName("M32041");
        Group group = isu.AddGroup(name);
        Student student = isu.AddStudent(group, "Name");

        Assert.Equal(student.Group, group);
        Assert.Contains(student, group.Students);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        var name = new GroupName("M32031");
        Group group = isu.AddGroup(name);
        for (int i = 0; i < Group.MaxStudents; i++)
            isu.AddStudent(group, "Максим Цай");

        Assert.Throws<IsuException>(() => isu.AddStudent(group, "Цаксим Май"));
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        // GroupName is too small
        Assert.ThrowsAny<Exception>(() => new GroupName("M999"));

        // GroupName has no faculty letter
        Assert.ThrowsAny<Exception>(() => new GroupName("9999"));

        // GroupName is empty
        Assert.ThrowsAny<Exception>(() => new GroupName(string.Empty));

        // GroupName is incorrect
        Assert.ThrowsAny<Exception>(() => new GroupName("M9999I"));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        var name = new GroupName("M32021");
        var anotherName = new GroupName("M32011");

        Group group = isu.AddGroup(name);
        Student student = isu.AddStudent(group, "Шалим Иванов");
        Group anotherGroup = isu.AddGroup(anotherName);

        Assert.Equal(student.Group, group);
        Assert.Contains(student, group.Students);

        isu.ChangeStudentGroup(student, anotherGroup);

        Assert.Equal(student.Group, anotherGroup);
        Assert.Contains(student, anotherGroup.Students);
        Assert.DoesNotContain(student, group.Students);
    }
}