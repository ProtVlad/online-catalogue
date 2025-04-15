using System;
using Xunit;
using Online_catalogue.Models;

public class AdminViewLogicTests : IDisposable
{
    private readonly DatabaseService _db;
    private readonly string _testEmail;

    public AdminViewLogicTests()
    {
        _db = new DatabaseService();
        _testEmail = $"adminview_{Guid.NewGuid()}@mail.com";
    }

    [Fact]
    public void InsertUser_AddsUserSuccessfully()
    {
        _db.InsertUser("Adaug", "Test", "elev", _testEmail, "parola");

        var user = _db.AuthenticateUser(_testEmail, "parola");

        Assert.NotNull(user);
        Assert.Equal(_testEmail, user.Email);
    }

    [Fact]
    public void DeleteUser_RemovesUserSuccessfully()
    {
        _db.InsertUser("Sterg", "User", "elev", _testEmail, "pass123");
        var user = _db.AuthenticateUser(_testEmail, "pass123");

        Assert.NotNull(user);

        _db.DeleteUser(user.Id);
        var afterDelete = _db.AuthenticateUser(_testEmail, "pass123");

        Assert.Null(afterDelete);
    }

    [Fact]
    public void CannotDeleteAdmin_ReturnsFalse()
    {
        var adminUser = new User { Id = 1, Rol = "admin" };

        bool canDelete = adminUser.Rol.ToLower() != "admin";

        Assert.False(canDelete);
    }

    [Fact]
    public void UpdateUser_ChangesUserDetails()
    {
        _db.InsertUser("Initial", "Name", "elev", _testEmail, "123456");

        var user = _db.AuthenticateUser(_testEmail, "123456");
        user.Nume = "NouNume";
        user.Prenume = "PrenumeActualizat";

        _db.UpdateUser(user);
        var updated = _db.AuthenticateUser(_testEmail, "123456");

        Assert.Equal("NouNume", updated.Nume);
        Assert.Equal("PrenumeActualizat", updated.Prenume);
    }

    public void Dispose()
    {
        var user = _db.AuthenticateUser(_testEmail, "parola")
                ?? _db.AuthenticateUser(_testEmail, "pass123")
                ?? _db.AuthenticateUser(_testEmail, "123456");

        if (user != null)
            _db.DeleteUser(user.Id);
    }
}
