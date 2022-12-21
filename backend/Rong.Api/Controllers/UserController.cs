using Microsoft.AspNetCore.Mvc;
using Rong.Core.Dtos.System;
using Rong.Core.Models;
using Rong.Infra.Services.System;

namespace Rong.Api.Controllers;

[ApiController]
[Route("/api/user")]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<Response>> Register([FromBody] Register dto)
    {
        var result = await _userService.Register(dto);
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<Response>> Login([FromBody] Login dto)
    {
        var result = await _userService.Login(dto);
        return Ok(result);
    }
}