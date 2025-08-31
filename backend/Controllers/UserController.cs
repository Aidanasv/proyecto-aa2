namespace Controllers;

using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[Route("users")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService service)
    {
        _userService = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NoContent();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        await _userService.AddAsync(user);
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User updatedUser)
    {
        await _userService.UpdateAsync(updatedUser, id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        await _userService.DeleteAsync(id);
        return NoContent();
    }

}