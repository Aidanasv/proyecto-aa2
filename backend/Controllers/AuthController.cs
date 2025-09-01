namespace Controllers;

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
    public IActionResult Register(UserRegister userRegister)
    {
           try
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var token = _authService.Register(userRegister);
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