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

    //Obtener usuarios
    [HttpGet]
    public async Task<ActionResult<List<UserRead>>> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        var usersRead = users.Select(user => new UserRead
        {
            Email = user.Email,
            Password = user.Password,
            Name = user.Name,
            Username = user.Username,
            BirthDate = user.BirthDate,
            CreateDate = user.CreateDate,
            LastLogin = user.LastLogin,
            Role = user.Role,
            Id = user.Id
        });
        return Ok(usersRead);
    }

    //Obtener usuario por id
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "No se encontró el usuario" });
        }

        var userRead = new UserRead
        {
            Email = user.Email,
            Password = user.Password,
            Name = user.Name,
            Username = user.Username,
            BirthDate = user.BirthDate,
            CreateDate = user.CreateDate,
            LastLogin = user.LastLogin,
            Role = user.Role,
            Id = user.Id
        };

        return Ok(userRead);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "Error al eliminar usuario" });
        }
        await _userService.DeleteAsync(id);
        return Ok(id);
    }

}