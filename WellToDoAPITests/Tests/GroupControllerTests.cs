using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WellToDo.Controllers;
using WellToDo.Models;
using WellToDoTests.Mocks;

namespace WellToDoTests.Controller;

[TestFixture]
public class GroupControllerTests
{
    private GroupController _controller;
    private DatabaseContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextMocker.GetDbContext();
        _controller = new GroupController(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        DbContextMocker.ResetDbContext(_dbContext);
    }

    [Test]
    public void GetGroups_ReturnsGroups()
    {
        // Act
        var result = _controller.GetGroups();
        var okResult = result.Result as OkObjectResult;
        var groups = okResult?.Value as IEnumerable<Group>;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(groups);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(2, groups.Count());
    }

    [Test]
    public void CreateGroup_ReturnsOk()
    {
        // Arrange
        var newGroup = new GroupRequest
        {
            Title = "New Group",
            Color = "Blue"
        };

        // Act
        var result = _controller.CreateGroup(newGroup);
        var okResult = result.Result as OkObjectResult;
        var group = okResult?.Value as Group;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(newGroup.Title, group.Title);
        Assert.AreEqual(newGroup.Color, group.Color);

        // Check if the group is added to the database
        Assert.AreEqual(3, _dbContext.Groups.Count());
        Assert.IsTrue(_dbContext.Groups.Any(
            group => group.Title == newGroup.Title 
            && group.Color == newGroup.Color
        ));
    }

    [Test]
    public void UpdateGroup_ExistingGroup_ReturnsOk()
    {
        // Arrange
        var groupId = _dbContext.Groups.First().Id;
        var editedGroup = new GroupRequest
        {
            Title = "Edited Group",
            Color = "Red"
        };

        // Act
        var result = _controller.UpdateGroup(groupId, editedGroup);
        var okResult = result.Result as OkObjectResult;
        var group = okResult?.Value as Group;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(group);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(groupId, group.Id);
        Assert.AreEqual(editedGroup.Title, group.Title);
        Assert.AreEqual(editedGroup.Color, group.Color);

        // Check if the group is updated in the database
        var updatedGroup = _dbContext.Groups.FirstOrDefault(group => group.Id == groupId);
        Assert.IsNotNull(updatedGroup);
        Assert.AreEqual(editedGroup.Title, updatedGroup.Title);
        Assert.AreEqual(editedGroup.Color, updatedGroup.Color);
    }

    [Test]
    public void UpdateGroup_NonexistentGroup_ReturnsNotFound()
    {
        // Arrange
        var groupId = 999; // Nonexistent group ID
        var editedGroup = new GroupRequest
        {
            Title = "Edited Group",
            Color = "Red"
        };

        // Act
        var result = _controller.UpdateGroup(groupId, editedGroup);
        var notFoundResult = result.Result as NotFoundObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.AreEqual("Group not found", notFoundResult.Value);
    }

    [Test]
    public void DeleteGroup_ExistingGroup_ReturnsNoContent()
    {
        // Arrange
        var groupId = _dbContext.Groups.First().Id;

        // Act
        var result = _controller.DeleteGroup(groupId);
        var noContentResult = result as NoContentResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(noContentResult);
        Assert.AreEqual(204, noContentResult.StatusCode);

        // Check if the group is deleted from the database
        Assert.AreEqual(1, _dbContext.Groups.Count());
        Assert.IsFalse(_dbContext.Groups.Any(group => group.Id == groupId));
    }

    [Test]
    public void DeleteGroup_NonexistentGroup_ReturnsNotFound()
    {
        // Arrange
        var groupId = 999; // Nonexistent group ID

        // Act
        var result = _controller.DeleteGroup(groupId);
        var notFoundResult = result as NotFoundObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
}
