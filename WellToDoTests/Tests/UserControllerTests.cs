using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WellToDo.Models;
using WellToDo.Controllers;
using WellToDoTests.Mocks;

namespace WellToDoTests.Controller;

[TestFixture]
public class UserControllerTests
{
    private UserController _controller;
    private DatabaseContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextMocker.GetDbContext();
        _controller = new UserController(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        DbContextMocker.ResetDbContext(_dbContext);
    }

    [Test]
    public void SignIn_ValidUsername_ShouldReturnUser()
    {
        // Arrange
        var request = new UserLoginRequest { Username = "john" };

        // Act
        var result = _controller.SignIn(request);
        var objectResult = result.Result as OkObjectResult;
        var user = objectResult?.Value as User;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(objectResult);
        Assert.IsNotNull(user);
        Assert.AreEqual(200, objectResult.StatusCode);
        Assert.AreEqual(1, user.Id);
        Assert.AreEqual("john", user.Username);
    }

    [Test]
    public void SignIn_InvalidUsername_ShouldReturnNotFound()
    {
        // Arrange
        var request = new UserLoginRequest { Username = "nonexistent" };

        // Act
        var result = _controller.SignIn(request);
        var objectResult = result.Result as NotFoundObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(404, objectResult.StatusCode);
        Assert.AreEqual("Username not found", objectResult.Value);
    }

    [Test]
    public void SignUp_ValidUser_ShouldReturnCreated()
    {
        // Arrange
        var newUser = new UserSignUpRequest
        {
            Username = "newuser",
            Firstname = "New",
            Lastname = "User"
        };

        // Act
        var result = _controller.SignUp(newUser);
        var objectResult = result.Result as CreatedAtActionResult;
        var user = objectResult?.Value as User;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(objectResult);
        Assert.IsNotNull(user);
        Assert.AreEqual(201, objectResult.StatusCode);
        Assert.AreEqual("newuser", user.Username);

        // Check if the user is added to the database
        Assert.AreEqual(2, _dbContext.Users.Count());
        Assert.IsTrue(_dbContext.Users.Any(u => u.Username == "newuser"));
    }

    [Test]
    public void SignUp_ExistingUser_ShouldReturnConflict()
    {
        // Arrange
        var existingUser = new UserSignUpRequest
        {
            Username = "john",
            Firstname = "John",
            Lastname = "Doe"
        };

        // Act
        var result = _controller.SignUp(existingUser);
        var objectResult = result.Result as ConflictObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(409, objectResult.StatusCode);
        Assert.AreEqual("Username already exists", objectResult.Value);

        // Check if the user is not added to the database
        Assert.AreEqual(1, _dbContext.Users.Count());
    }
}