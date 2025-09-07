namespace Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[ApiController]
[Route("auth")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Login")]
    public IActionResult Login(UserAuth userAuth)
    {
        try
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var token = _authService.Login(userAuth);
            if (token == "")
            {
                return BadRequest
                ("Usuario o clave incorrecta");
            }
            return Ok(token);
        }
        catch (KeyNotFoundException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest
            ("Error generating the token: " + ex.Message);
        }
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserRegisterPassword userRegister)
    {
        try
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (userRegister.Password != userRegister.PasswordValidate)
            {
                return BadRequest
                ("Las contraseñas deben coincidir");
            }
            var token = await _authService.Register(userRegister, Role.Client);
            return Ok(token);
        }
        catch (KeyNotFoundException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest
            ("Error generating the token: " + ex.Message);
        }
    }

    [Authorize(Roles = Role.Admin)]
    [HttpPost("Register/Admin")]
    public async Task<IActionResult> RegisterAdmin(UserRegisterPassword userRegister)
    {
        try
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (userRegister.Password != userRegister.PasswordValidate)
            {
                return BadRequest
                ("Las contraseñas deben coincidir");
            }
            var token = await _authService.Register(userRegister, Role.Admin);
            return Ok(token);
        }
        catch (KeyNotFoundException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest
            ("Error generating the token: " + ex.Message);
        }
    }


}