using System;
using Xunit;
using Online_catalogue.Models;
using System.Linq;
using System.Collections.Generic;

public class TeacherHomeViewLogicTests : IDisposable
{
    private readonly DatabaseService _db;
    private readonly string _email;
    private User _prof;
    private List<int> _createdCourseIds = new List<int>();

    public TeacherHomeViewLogicTests()
    {
        _db = new DatabaseService();
        _email = $"prof_{Guid.NewGuid()}@mail.com";
        _db.InsertUser("Test", "Prof", "profesor", _email, "parola123");
        _prof = _db.AuthenticateUser(_email, "parola123");
    }

    [Fact]
    public void AddCourse_AssignsCourseToTeacher()
    {
        var curs = new Curs { NumeCurs = $"TestCurs_{Guid.NewGuid()}", Descriere = "Test descriere" };
        _db.AddCourse(curs);
        _db.AddUserCourseLink(_prof.Id, curs.Id);

        _createdCourseIds.Add(curs.Id);

        var courses = _db.GetCoursesForTeacher(_prof.Id);
        Assert.Contains(courses, c => c.Id == curs.Id);
    }

    [Fact]
    public void DeleteCourse_RemovesCourse()
    {
        var curs = new Curs { NumeCurs = $"ToDelete_{Guid.NewGuid()}", Descriere = "Temp" };
        _db.AddCourse(curs);
        _db.AddUserCourseLink(_prof.Id, curs.Id);

        _db.DeleteCourse(curs.Id);
        var courses = _db.GetCoursesForTeacher(_prof.Id);

        Assert.DoesNotContain(courses, c => c.Id == curs.Id);
    }

    [Fact]
    public void GetCoursesForTeacher_ReturnsOnlyAssignedCourses()
    {
        var curs1 = new Curs { NumeCurs = $"ProfCurs1_{Guid.NewGuid()}", Descriere = "A" };
        var curs2 = new Curs { NumeCurs = $"ProfCurs2_{Guid.NewGuid()}", Descriere = "B" };

        _db.AddCourse(curs1);
        _db.AddCourse(curs2);
        _db.AddUserCourseLink(_prof.Id, curs2.Id);

        _createdCourseIds.Add(curs1.Id);
        _createdCourseIds.Add(curs2.Id);

        var courses = _db.GetCoursesForTeacher(_prof.Id);
        Assert.DoesNotContain(courses, c => c.Id == curs1.Id);
        Assert.Contains(courses, c => c.Id == curs2.Id);
    }

    [Fact]
    public void UpdatePassword_ChangesSuccessfully()
    {
        string newPassword = "n0u@Pa$$";
        _db.UpdateUserPassword(_prof.Id, newPassword);

        var user = _db.AuthenticateUser(_email, newPassword);

        Assert.NotNull(user);
        Assert.Equal(_email, user.Email);
    }

    public void Dispose()
    {
        foreach (var courseId in _createdCourseIds)
        {
            _db.DeleteCourse(courseId);
        }

        if (_prof != null)
        {
            _db.DeleteUser(_prof.Id);
        }
    }
}
