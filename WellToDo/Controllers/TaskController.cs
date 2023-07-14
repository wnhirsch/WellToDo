using Microsoft.AspNetCore.Mvc;
using WellToDo.Models;
using TaskModel = WellToDo.Models.Task;

namespace WellToDo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public TaskController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskModel>), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<TaskModel>> GetTasks(
        [FromHeader] string userName,
        [FromQuery] TaskFilterRequest request,
        int page = 1, 
        int pageSize = 10
    ) {
        // Authentication
        int userId = GetLoggedUserId(userName);
        if (userId == 0)
        {
            return NotFound("User not found");
        }

        // Query
        var query = _dbContext.Tasks
            .Where(task => task.UserId == userId);

        // Apply filters
        if (!string.IsNullOrEmpty(request.sentence))
        {
            string[] words = request.sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                query = query.Where(task => task.Title.ToUpper().Contains(word.ToUpper())
                        || (task.Description != null && task.Description.ToUpper().Contains(word.ToUpper())));
            }
        }
        if (!string.IsNullOrEmpty(request.date))
        {
            DateTime startDate = DateTime.ParseExact(request.date, "yyyy'-'MM'-'dd", null);
            DateTime endDate = startDate.AddDays(1);
            query = query.Where(task => task.Date >= startDate && task.Date < endDate);
        }
        if (request.priority.HasValue)
        {
            query = query.Where(task => task.Priority == request.priority);
        }
        if (request.isChecked.HasValue)
        {
            query = query.Where(task => task.IsChecked == request.isChecked);
        }
        if (request.isFlagged.HasValue)
        {
            query = query.Where(task => task.IsFlagged == request.isFlagged);
        }
        if (request.groupId.HasValue)
        {
            query = query.Where(task => task.GroupId == request.groupId);
        }

        var tasks = query
            .OrderBy(task => task.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(tasks);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<TaskModel> CreateTask([FromHeader] string userName, TaskRequest newTask)
    {
        // Authentication
        int userId = GetLoggedUserId(userName);
        if (userId == 0)
        {
            return NotFound("User not found");
        }

        // Operation
        var group = _dbContext.Groups.FirstOrDefault(group => group.Id == newTask.GroupId && group.UserId == userId);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        var task = new TaskModel
        {
            Title = newTask.Title,
            Description = newTask.Description,
            Date = newTask.Date,
            Priority = newTask.Priority,
            Url = newTask.Url,
            IsChecked = newTask.IsChecked,
            IsFlagged = newTask.IsFlagged,
            GroupId = group.Id,
            UserId = userId
        };

        _dbContext.Tasks.Add(task);
        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(typeof(TaskModel), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<TaskModel> UpdateTask([FromHeader] string userName, int id, TaskRequest editedTask)
    {
        // Authentication
        int userId = GetLoggedUserId(userName);
        if (userId == 0)
        {
            return NotFound("User not found");
        }

        // Operation
        var group = _dbContext.Groups.FirstOrDefault(group => group.Id == editedTask.GroupId && group.UserId == userId);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        var task = _dbContext.Tasks.FirstOrDefault(task => task.Id == id && task.UserId == userId);
        if (task == null)
        {
            return NotFound("Task not found");
        }

        task.Title = editedTask.Title;
        task.Description = editedTask.Description;
        task.Date = editedTask.Date;
        task.Priority = editedTask.Priority;
        task.Url = editedTask.Url;
        task.IsChecked = editedTask.IsChecked;
        task.IsFlagged = editedTask.IsFlagged;
        task.GroupId = group.Id;
        task.UserId = userId;

        _dbContext.Tasks.Update(task);
        _dbContext.SaveChanges();

        return Ok(task);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult DeleteTask([FromHeader] string userName, int id)
    {
        // Authentication
        int userId = GetLoggedUserId(userName);
        if (userId == 0)
        {
            return NotFound("User not found");
        }

        // Operation
        var task = _dbContext.Tasks.FirstOrDefault(task => task.Id == id && task.UserId == userId);
        if (task == null)
        {
            return NotFound("Task not found");
        }

        _dbContext.Tasks.Remove(task);
        _dbContext.SaveChanges();

        return NoContent();
    }

    private int GetLoggedUserId(string userName)
    {
        int userId = _dbContext.Users
            .Where(u => u.Username == userName)
            .Select(u => u.Id)
            .FirstOrDefault();

        return userId;
    }
}