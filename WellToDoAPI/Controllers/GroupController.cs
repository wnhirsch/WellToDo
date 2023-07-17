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
    public ActionResult<IEnumerable<Group>> GetGroups(int page = 1, int pageSize = 10)
    {
        // Operation
        var groups = _dbContext.Groups
            .OrderBy(group => group.Title.ToUpper())
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(groups);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<Group> CreateGroup(GroupRequest newGroup)
    {
        // Operation
        var group = new Group
        {
            Title = newGroup.Title,
            Color = newGroup.Color
        };

        _dbContext.Groups.Add(group);
        _dbContext.SaveChanges();

        return Ok(group);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<Group> UpdateGroup(int id, GroupRequest editedGroup)
    {
        // Validation
        var group = _dbContext.Groups.FirstOrDefault(group => group.Id == id);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        // Operation 
        group.Title = editedGroup.Title;
        group.Color = editedGroup.Color;

        _dbContext.Groups.Update(group);
        _dbContext.SaveChanges();

        return Ok(group);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult DeleteGroup(int id)
    {
        // Validation
        var group = _dbContext.Groups.FirstOrDefault(group => group.Id == id);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        // Operation
        _dbContext.Groups.Remove(group);
        _dbContext.SaveChanges();

        return NoContent();
    }
}