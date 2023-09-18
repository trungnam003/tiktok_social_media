using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Features.Users.Queries.Login;
using Tiktok.API.Infrastructure;
using Tiktok.API.Infrastructure.Validators;

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
        var result =await _mediator.Send(login);
        return Ok(result);
    }
}