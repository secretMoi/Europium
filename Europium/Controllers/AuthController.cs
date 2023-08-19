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
            return Ok(new { Token = token, RefreshToken = await _authService.GenerateRefreshToken(login.Username) });

        return Unauthorized();
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var (principal, jwtToken) = _authService.ValidateRefreshToken(refreshToken);
        if (principal == null)
            return Unauthorized();

        var newJwtToken = _authService.GenerateJwtToken(principal.Identity.Name);
        var newRefreshToken = await _authService.GenerateRefreshToken(principal.Identity.Name);

        return Ok(new
        {
            Token = newJwtToken,
            RefreshToken = newRefreshToken
        });
    }
    
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}