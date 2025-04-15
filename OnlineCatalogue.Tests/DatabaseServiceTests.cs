using Xunit;
using Online_catalogue.Models;
using System;

public class DatabaseServiceTests : IDisposable
{
    private readonly DatabaseService _service;
    private string _testEmail;

    public DatabaseServiceTests()
    {
        _service = new DatabaseService();
        _testEmail = $"testuser_{Guid.NewGuid()}@mail.com";
    }

    [Fact]
    public void InsertUser_ThenAuthenticateUser_Success()
    {
        // Arrange
        string parola = "parola123";
        _service.InsertUser("NumeTest", "PrenumeTest", "elev", _testEmail, parola);

        // Act
        var user = _service.AuthenticateUser(_testEmail, parola);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(_testEmail, user.Email);
    }

    [Fact]
    public void InsertAndDeleteUser_UserIsRemoved()
    {
        // Arrange
        _service.InsertUser("ToDelete", "User", "elev", _testEmail, "delete123");
        var user = _service.AuthenticateUser(_testEmail, "delete123");
        Assert.NotNull(user);

        // Act
        _service.DeleteUser(user.Id);

        // Assert
        var afterDelete = _service.AuthenticateUser(_testEmail, "delete123");
        Assert.Null(afterDelete);
    }

    [Fact]
    public void AddNota_ThenRetrieve_NotaReturned()
    {
        // Arrange
        _service.InsertUser("Note", "User", "elev", _testEmail, "note123");
        var user = _service.AuthenticateUser(_testEmail, "note123");
        var curs = new Curs { NumeCurs = $"CursTest_{Guid.NewGuid()}", Descriere = "Test descriere" };
        _service.AddCourse(curs);
        _service.AddUserCourseLink(user.Id, curs.Id);

        var nota = new Nota
        {
            IdUser = user.Id,
            IdCurs = curs.Id,
            NotaValoare = 9
        };

        _service.AddNota(nota);

        // Act
        var noteList = _service.GetNote(user.Id, curs.Id);

        // Assert
        Assert.NotEmpty(noteList);
        Assert.Contains(noteList, n => n.NotaValoare == 9);
    }

    [Fact]
    public void AddCourse_ThenGetCourses_CourseIsPresent()
    {
        // Arrange
        var courseName = $"Course_{Guid.NewGuid()}";
        var curs = new Curs { NumeCurs = courseName, Descriere = "Test" };
        _service.AddCourse(curs);

        // Act
        var courses = _service.GetCourses();

        // Assert
        Assert.Contains(courses, c => c.NumeCurs == courseName);
    }

    [Fact]
    public void UpdateUserPassword_ChangesPasswordSuccessfully()
    {
        // Arrange
        string initialPass = "oldPass123";
        string newPass = "newPass456";
        _service.InsertUser("Pass", "Changer", "elev", _testEmail, initialPass);
        var user = _service.AuthenticateUser(_testEmail, initialPass);

        // Act
        _service.UpdateUserPassword(user.Id, newPass);
        var updatedUser = _service.AuthenticateUser(_testEmail, newPass);

        // Assert
        Assert.NotNull(updatedUser);
        Assert.Equal(_testEmail, updatedUser.Email);
    }

    [Fact]
    public void AddUserToCourse_UserAppearsInCourse()
    {
        // Arrange
        _service.InsertUser("Student", "Course", "elev", _testEmail, "elevpass");
        var user = _service.AuthenticateUser(_testEmail, "elevpass");
        var curs = new Curs { NumeCurs = $"Algebra_{Guid.NewGuid()}", Descriere = "Matematica" };
        _service.AddCourse(curs);

        // Act
        _service.AddUserToCourse(user.Id, curs.Id);
        var usersInCurs = _service.GetUsersByCourseId(curs.Id);

        // Assert
        Assert.Contains(usersInCurs, u => u.Id == user.Id);
    }

    [Fact]
    public void DeleteCourse_RemovesCourseSuccessfully()
    {
        // Arrange
        var curs = new Curs { NumeCurs = $"ToDelete_{Guid.NewGuid()}", Descriere = "Temporary" };
        _service.AddCourse(curs);
        var courseId = curs.Id;

        // Act
        _service.DeleteCourse(courseId);
        var course = _service.GetCourseById(courseId);

        // Assert
        Assert.Null(course);
    }

    [Fact]
    public void AddTeacherToCourse_TeacherAppearsInCourse()
    {
        // Arrange
        var email = $"teacher_{Guid.NewGuid()}@mail.com";
        _service.InsertUser("Prof", "Test", "profesor", email, "teachpass");
        var teacher = _service.AuthenticateUser(email, "teachpass");
        var curs = new Curs { NumeCurs = $"Fizica_{Guid.NewGuid()}", Descriere = "Lec?ii" };
        _service.AddCourse(curs);

        // Act
        _service.AddUserCourseLink(teacher.Id, curs.Id);
        var teacherCourses = _service.GetCoursesForTeacher(teacher.Id);

        // Assert
        Assert.Contains(teacherCourses, c => c.Id == curs.Id);
    }

    [Fact]
    public void GetEleviDisponibili_ReturnsStudentNotInCourse()
    {
        // Arrange
        var curs = new Curs { NumeCurs = $"Geografie_{Guid.NewGuid()}", Descriere = "Continent" };
        _service.AddCourse(curs);

        var email = $"elev_{Guid.NewGuid()}@mail.com";
        _service.InsertUser("Geo", "Student", "elev", email, "pass123");
        var elev = _service.AuthenticateUser(email, "pass123");

        // Act
        var disponibili = _service.GetEleviDisponibili(curs.Id);

        // Assert
        Assert.Contains(disponibili, u => u.Id == elev.Id);
    }

    [Fact]
    public void GetEleviPentruCurs_ReturnsOnlyStudents()
    {
        // Arrange
        var curs = new Curs { NumeCurs = $"Lit_{Guid.NewGuid()}", Descriere = "Literatura" };
        _service.AddCourse(curs);

        var emailElev = $"elevLit_{Guid.NewGuid()}@mail.com";
        var emailProf = $"profLit_{Guid.NewGuid()}@mail.com";
        _service.InsertUser("Lit", "Student", "elev", emailElev, "litpass");
        _service.InsertUser("Lit", "Prof", "profesor", emailProf, "litpass");

        var elev = _service.AuthenticateUser(emailElev, "litpass");
        var prof = _service.AuthenticateUser(emailProf, "litpass");

        _service.AddUserToCourse(elev.Id, curs.Id);
        _service.AddUserToCourse(prof.Id, curs.Id);

        // Act
        var elevi = _service.GetEleviPentruCurs(curs.Id);

        // Assert
        Assert.Single(elevi);
        Assert.Equal("elev", elevi[0].Rol);
    }

    [Fact]
    public void RemoveStudentFromCourse_RemovesSuccessfully()
    {
        // Arrange
        var email = $"removeme_{Guid.NewGuid()}@mail.com";
        _service.InsertUser("Remove", "This", "elev", email, "rmpass");
        var elev = _service.AuthenticateUser(email, "rmpass");

        var curs = new Curs { NumeCurs = $"Birotica_{Guid.NewGuid()}", Descriere = "Office stuff" };
        _service.AddCourse(curs);
        _service.AddUserToCourse(elev.Id, curs.Id);

        // Act
        _service.RemoveStudentFromCourse(elev.Id, curs.Id);
        var eleviRamasi = _service.GetUsersByCourseId(curs.Id);

        // Assert
        Assert.DoesNotContain(eleviRamasi, u => u.Id == elev.Id);
    }


    public void Dispose()
    {
        // Cleanup test user and related data
        var user = _service.AuthenticateUser(_testEmail, "parola123")
            ?? _service.AuthenticateUser(_testEmail, "delete123")
            ?? _service.AuthenticateUser(_testEmail, "note123");

        if (user != null)
        {
            _service.DeleteUser(user.Id);
        }
    }
}
