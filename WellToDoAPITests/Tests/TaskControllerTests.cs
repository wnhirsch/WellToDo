using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WellToDo.Controllers;
using WellToDo.Models;
using WellToDoTests.Mocks;
using TaskModel = WellToDo.Models.Task;

namespace WellToDoTests.Controller;

[TestFixture]
public class TaskControllerTests
{
    private TaskController _controller;
    private DatabaseContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = DbContextMocker.GetDbContext();
        _controller = new TaskController(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        DbContextMocker.ResetDbContext(_dbContext);
    }

    [Test]
    public void GetTasks_ReturnsTasks()
    {
        // Arrange
        var request = new TaskFilterRequest
        {
            sentence = "task",
            date = "2023-07-12",
            priority = TaskPriority.High,
            isChecked = true,
            isFlagged = false,
            groupId = 2
        };
        var page = 1;
        var pageSize = 10;

        // Act
        var result = _controller.GetTasks(request, page, pageSize);
        var okResult = result.Result as OkObjectResult;
        var tasks = okResult?.Value as IEnumerable<TaskModel>;
        
        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(tasks);
        Assert.AreEqual(1, tasks.Count());
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [Test]
    public void CreateTask_ValidData_ReturnsOk()
    {
        // Arrange
        var newTask = new TaskRequest
        {
            Title = "New Task",
            Description = "Description",
            Date = DateTime.ParseExact("2023-07-12", "yyyy'-'MM'-'dd", null),
            Priority = TaskPriority.Low,
            Url = "http://example.com",
            IsChecked = false,
            IsFlagged = true,
            GroupId = 1
        };

        // Act
        var result = _controller.CreateTask(newTask);
        var okResult = result.Result as OkObjectResult;
        var task = okResult?.Value as TaskModel;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(task);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(newTask.Title, task.Title);
        Assert.AreEqual(newTask.Description, task.Description);
        Assert.AreEqual(newTask.Date, task.Date);
        Assert.AreEqual(newTask.Priority, task.Priority);
        Assert.AreEqual(newTask.Url, task.Url);
        Assert.AreEqual(newTask.IsChecked, task.IsChecked);
        Assert.AreEqual(newTask.IsFlagged, task.IsFlagged);
        Assert.AreEqual(newTask.GroupId, task.GroupId);

        // Check if the group is added to the database
        Assert.AreEqual(3, _dbContext.Tasks.Count());
        Assert.IsTrue(_dbContext.Tasks.Any(
            task => task.Title == newTask.Title
            && task.Description == newTask.Description
            && task.Date == newTask.Date
            && task.Priority == newTask.Priority
            && task.Url == newTask.Url
            && task.IsChecked == newTask.IsChecked
            && task.IsFlagged == newTask.IsFlagged
            && task.GroupId == newTask.GroupId
        ));
    }

    [Test]
    public void UpdateTask_ValidData_ReturnsOk()
    {
        // Arrange
        var taskId = _dbContext.Tasks.First().Id;
        var editedTask = new TaskRequest
        {
            Title = "Updated Task",
            Description = "Updated Description",
            Date = DateTime.ParseExact("2023-07-12", "yyyy'-'MM'-'dd", null),
            Priority = TaskPriority.High,
            Url = "http://example.com",
            IsChecked = true,
            IsFlagged = false,
            GroupId = 1
        };

        // Act
        var result = _controller.UpdateTask(taskId, editedTask);
        var okResult = result.Result as OkObjectResult;
        var task = okResult?.Value as TaskModel;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(task);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(taskId, task.Id);
        Assert.AreEqual(editedTask.Title, task.Title);
        Assert.AreEqual(editedTask.Description, task.Description);
        Assert.AreEqual(editedTask.Date, task.Date);
        Assert.AreEqual(editedTask.Priority, task.Priority);
        Assert.AreEqual(editedTask.Url, task.Url);
        Assert.AreEqual(editedTask.IsChecked, task.IsChecked);
        Assert.AreEqual(editedTask.IsFlagged, task.IsFlagged);
        Assert.AreEqual(editedTask.GroupId, task.GroupId);

        // Check if the group is updated in the database
        var updatedTask = _dbContext.Tasks.FirstOrDefault(task => task.Id == taskId);
        Assert.IsNotNull(updatedTask);
        Assert.AreEqual(editedTask.Title, updatedTask.Title);
        Assert.AreEqual(editedTask.Description, updatedTask.Description);
        Assert.AreEqual(editedTask.Date, updatedTask.Date);
        Assert.AreEqual(editedTask.Priority, updatedTask.Priority);
        Assert.AreEqual(editedTask.Url, updatedTask.Url);
        Assert.AreEqual(editedTask.IsChecked, updatedTask.IsChecked);
        Assert.AreEqual(editedTask.IsFlagged, updatedTask.IsFlagged);
        Assert.AreEqual(editedTask.GroupId, updatedTask.GroupId);
    }

    [Test]
    public void UpdateTask_NonexistentTask_ReturnsNotFound()
    {
        // Arrange
        var taskId = 999; // Nonexistent task ID
        var editedTask = new TaskRequest
        {
            Title = "Updated Task",
            Description = "Updated Description",
            Date = DateTime.ParseExact("2023-07-12", "yyyy'-'MM'-'dd", null),
            Priority = TaskPriority.High,
            Url = "http://example.com",
            IsChecked = true,
            IsFlagged = false,
            GroupId = 1
        };

        // Act
        var result = _controller.UpdateTask(taskId, editedTask);
        var notFoundResult = result.Result as NotFoundObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.AreEqual("Task not found", notFoundResult.Value);
    }

    [Test]
    public void DeleteTask_ExistingTask_ReturnsNoContent()
    {
        // Arrange
        var taskId = _dbContext.Tasks.First().Id;

        // Act
        var result = _controller.DeleteTask(taskId);
        var noContentResult = result as NoContentResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(noContentResult);
        Assert.AreEqual(204, noContentResult.StatusCode);

        // Check if the group is deleted from the database
        Assert.AreEqual(1, _dbContext.Tasks.Count());
        Assert.IsFalse(_dbContext.Tasks.Any(task => task.Id == taskId));
    }

    [Test]
    public void DeleteTask_NonexistentTask_ReturnsNotFound()
    {
        // Arrange
        var taskId = 999; // Nonexistent task ID

        // Act
        var result = _controller.DeleteTask(taskId);
        var notFoundResult = result as NotFoundObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
}
