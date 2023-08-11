using Europium.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Europium.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var token = await _authService.Login(login.Username, login.Password);
        if(token is not null)
            return Ok(token);

        return Unauthorized();
    }
    
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}