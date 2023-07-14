using Microsoft.AspNetCore.Mvc;
using WellToDo.Models;

namespace WellToDo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public UserController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("signin")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public ActionResult<User> SignIn(UserLoginRequest request)
    {
        var user = _dbContext.Users.FirstOrDefault(user => user.Username == request.Username);
        if (user == null)
        {
            return NotFound("Username not found");
        }

        return Ok(user);
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    public ActionResult<User> SignUp(UserSignUpRequest newUser)
    {
        var existingUser = _dbContext.Users.FirstOrDefault(user => user.Username == newUser.Username);
        if (existingUser != null)
        {
            return Conflict("Username already exists");
        }

        var user = new User
        {
            Username = newUser.Username,
            Firstname = newUser.Firstname,
            Lastname = newUser.Lastname
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(SignIn), new { username = user.Username }, user);
    }
}

