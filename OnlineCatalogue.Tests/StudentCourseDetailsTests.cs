using System;
using System.Linq;
using Xunit;
using Online_catalogue.Models;
using System.Collections.Generic;

public class StudentCourseDetailsTests : IDisposable
{
    private readonly DatabaseService _db;
    private readonly string _email;
    private User _student;
    private Curs _curs;

    public StudentCourseDetailsTests()
    {
        _db = new DatabaseService();
        _email = $"student_{Guid.NewGuid()}@mail.com";
        _db.InsertUser("Test", "Student", "elev", _email, "testpass");
        _student = _db.AuthenticateUser(_email, "testpass");

        _curs = new Curs { NumeCurs = $"Curs_{Guid.NewGuid()}", Descriere = "Descriere test" };
        _db.AddCourse(_curs);
        _db.AddUserToCourse(_student.Id, _curs.Id);
    }

    [Fact]
    public void AddNotaForStudentAndCourse_ReturnsCorrectNote()
    {
        // Arrange
        var nota = new Nota { IdUser = _student.Id, IdCurs = _curs.Id, NotaValoare = 8 };
        _db.AddNota(nota);

        // Act
        var note = _db.GetNotesForStudent(_student.Id)
                      .Where(n => n.IdCurs == _curs.Id)
                      .ToList();

        // Assert
        Assert.Single(note);
        Assert.Equal(8, note.First().NotaValoare);
    }

    [Fact]
    public void CalculateMediaForMultipleNotes_ReturnsCorrectAverage()
    {
        // Arrange
        _db.AddNota(new Nota { IdUser = _student.Id, IdCurs = _curs.Id, NotaValoare = 7 });
        _db.AddNota(new Nota { IdUser = _student.Id, IdCurs = _curs.Id, NotaValoare = 9 });

        // Act
        var note = _db.GetNotesForStudent(_student.Id)
                      .Where(n => n.IdCurs == _curs.Id)
                      .ToList();

        var media = note.Any() ? note.Average(n => n.NotaValoare) : 0;

        // Assert
        Assert.Equal(8, media);
    }

    [Fact]
    public void GetNotes_WhenNoNotes_ReturnsEmptyListAndMediaZero()
    {
        // Act
        var note = _db.GetNotesForStudent(_student.Id)
                      .Where(n => n.IdCurs == _curs.Id)
                      .ToList();

        var media = note.Any() ? note.Average(n => n.NotaValoare) : 0;

        // Assert
        Assert.Empty(note);
        Assert.Equal(0, media);
    }

    public void Dispose()
    {
        // Cleanup utilizator și curs
        if (_student != null)
        {
            _db.RemoveStudentFromCourse(_student.Id, _curs.Id);
            _db.DeleteUser(_student.Id);
        }

        _db.DeleteCourse(_curs.Id);
    }
}
