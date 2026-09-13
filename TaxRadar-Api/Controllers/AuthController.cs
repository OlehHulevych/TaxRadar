using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxRadar_Application.Queries.Auth;

namespace TaxRadar_backned;

[ApiController]
[Route("api/auth")]
public class AuthController(ISender sender):ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterQuery query)
    {
        var result = await sender.Send(query);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        var result = await sender.Send(query);
        return Ok(result);
    }

    
    [HttpGet]
    public async Task<IActionResult> Logout([FromBody] RevokeRefreshTokenQuery query)
    {
        await sender.Send(query);
        return Ok("User is loged out");
    }

    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenQuery query)
    {
        var result = await sender.Send(query);
        return Ok(result);
    }
}