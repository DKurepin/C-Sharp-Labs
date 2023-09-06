using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Services;
using Isu.Extra.Tools;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraServiceTest
{
    private IsuExtraService isu = new IsuExtraService();

    [Fact]
    public void Sсhedule_IntersectsAreIdentified()
    {
        var groupName = new GroupName("M3204");
        var group = isu.AddGroup(groupName);
        var student = isu.AddStudent(group, "Курилл Куропатов");
        var lesson = new Lesson(new TimeOnly(10, 00), new TimeOnly(11, 30), "Перекур", "Четверг");
        var lesson1 = new Lesson(new TimeOnly(11, 29), new TimeOnly(13, 00), "Уборка окурков", "Четверг");
        var lesson2 = new Lesson(new TimeOnly(11, 31), new TimeOnly(13, 00), "Уборка укурков", "Четверг");

        group.Schedule.AddLesson(lesson);
        var ognp = isu.AddOgnp("K2");
        var flow1 = isu.AddFlow(ognp, "K2.1");
        flow1.Schedule.AddLesson(lesson1);
        var flow2 = isu.AddFlow(ognp, "K2.2");
        flow2.Schedule.AddLesson(lesson2);
        var exception = Record.Exception(() => isu.SubscribeStudentToFlow(flow2, student));

        Assert.Throws<IsuExtraException>(() => isu.SubscribeStudentToFlow(flow1, student));
        Assert.Null(exception);
    }

    [Fact]
    public void Student_RespectsOgnpLimit()
    {
        var groupName = new GroupName("M3203");

        var group = isu.AddGroup(groupName);
        var student = isu.AddStudent(group, "Имя Фамилия");
        var ognp1 = isu.AddOgnp("M3");
        var flow1 = isu.AddFlow(ognp1, "M3.2");

        Assert.Throws<IsuExtraException>(() => isu.SubscribeStudentToFlow(flow1, student));
    }

    [Fact]
    public void Ognp_OgnpLimitEnforced()
    {
        var groupName = new GroupName("M3202");
        var group = isu.AddGroup(groupName);
        var student = isu.AddStudent(group, "Фимя Амилия");

        var ognp2 = isu.AddOgnp("A3");
        var flow2 = isu.AddFlow(ognp2, "A3.2");
        var ognp3 = isu.AddOgnp("B3");
        var flow3 = isu.AddFlow(ognp3, "B3.2");
        var ognp4 = isu.AddOgnp("C3");
        var flow4 = isu.AddFlow(ognp4, "C3.2");

        var exception = Record.Exception(() => isu.SubscribeStudentToFlow(flow2, student));
        Assert.Null(exception);
        exception = Record.Exception(() => isu.SubscribeStudentToFlow(flow3, student));
        Assert.Null(exception);
        Assert.Throws<IsuExtraException>(() => isu.SubscribeStudentToFlow(flow4, student));
    }
}