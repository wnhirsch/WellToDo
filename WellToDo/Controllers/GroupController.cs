using Microsoft.AspNetCore.Mvc;
using WellToDo.Models;

namespace WellToDo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public GroupController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Group>), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<Group>> GetGroups([FromHeader] string userName, int page = 1, int pageSize = 10)
    {
        // Authentication
        int userId = GetLoggedUserId(userName);
        if (userId == 0)
        {
            return NotFound("User not found");
        }

        // Operation
        var groups = _dbContext.Groups
            .Where(group => group.UserId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(groups);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<Group> CreateGroup([FromHeader] string userName, GroupRequest newGroup)
    {
        // Authentication
        int userId = GetLoggedUserId(userName);
        if (userId == 0)
        {
            return NotFound("User not found");
        }

        // Operation
        var group = new Group
        {
            Title = newGroup.Title,
            Color = newGroup.Color,
            UserId = userId
        };

        _dbContext.Groups.Add(group);
        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<Group> UpdateGroup([FromHeader] string userName, int id, GroupRequest editedGroup)
    {
        // Authentication
        int userId = GetLoggedUserId(userName);
        if (userId == 0)
        {
            return NotFound("User not found");
        }

        // Operation
        var group = _dbContext.Groups.FirstOrDefault(group => group.Id == id && group.UserId == userId);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        group.Title = editedGroup.Title;
        group.Color = editedGroup.Color;

        _dbContext.Groups.Update(group);
        _dbContext.SaveChanges();

        return Ok(group);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult DeleteGroup([FromHeader] string userName, int id)
    {
        // Authentication
        int userId = GetLoggedUserId(userName);
        if (userId == 0)
        {
            return NotFound("User not found");
        }

        // Operation
        var group = _dbContext.Groups.FirstOrDefault(group => group.Id == id && group.UserId == userId);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        _dbContext.Groups.Remove(group);
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