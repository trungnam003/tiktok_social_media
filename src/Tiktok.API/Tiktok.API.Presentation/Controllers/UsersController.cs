using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tiktok.API.Application.Features.Users.Commands.Register;
using Tiktok.API.Application.Features.Users.Queries.Login;


namespace Tiktok.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginQuery login)
    {
        var result = await _mediator.Send(login);
        return Ok(result);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand model)
    {
        var result = await _mediator.Send(model);
        return Ok(result);
    }

    [HttpGet("profile")]
    [Authorize]
    public IActionResult GetProfile()
    {
        return this.Ok(new { message = "API is under development" });
    }

    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword()
    {
        return this.Ok(new { message = "API is under development" });
    }

    [HttpPatch("change-password")]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return this.Ok(new { message = "API is under development" });
    }

    [HttpPatch("update-profile")]
    [Authorize]
    public IActionResult UpdateProfile()
    {
        return this.Ok(new { message = "API is under development" });
    }
}