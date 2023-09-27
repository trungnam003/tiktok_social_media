using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Features.Users.Commands.ChangePassword;
using Tiktok.API.Application.Features.Users.Commands.CreateNewForgottenPassword;
using Tiktok.API.Application.Features.Users.Commands.FollowUser;
using Tiktok.API.Application.Features.Users.Commands.ForgotPassword;
using Tiktok.API.Application.Features.Users.Commands.Register;
using Tiktok.API.Application.Features.Users.Commands.UnfollowUser;
using Tiktok.API.Application.Features.Users.Queries.GetFollowersWithPaging;
using Tiktok.API.Application.Features.Users.Queries.GetFollowingWithPaging;
using Tiktok.API.Application.Features.Users.Queries.Login;
using Tiktok.API.Application.Features.Users.Queries.VerifyOtp;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;


namespace Tiktok.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <remarks>email: thtn.1611.dev@gmail.com password: 1234</remarks>
    [HttpPost("login")]
    [SwaggerOperation(summary: "Login user")]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiSuccessResult<RegisterResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] UserLoginQuery login)
    {
        var result = await _mediator.Send(login);
        return Ok(result);
    }

    [HttpPost("register")]
    [SwaggerOperation(summary: "Register new user")]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiSuccessResult<RegisterResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand model)
    {
        var result = await _mediator.Send(model);
        return Ok(result);
    }

    [HttpPost("forgot-password")]
    [SwaggerOperation(summary: "Request forgot password and send OTP to user's email")]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiSuccessResult<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPatch("forgot-password")]
    [SwaggerOperation(summary: "Update new password for user using OTP")]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiSuccessResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status410Gone)]
    public async Task<IActionResult> CreateNewForgottenPassword([FromBody] CreateNewForgottenPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("verify-otp")]
    [SwaggerOperation(summary: "Verify OTP")]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status410Gone)]
    [ProducesResponseType(typeof(ApiSuccessResult<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpQuery query)
    {
        var result = await _mediator.Send(query);
        return Accepted(result);
    }
    
    [HttpPatch("change-password")]
    [SwaggerOperation(summary: "Change password of current user")]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiSuccessResult<string>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
    {
        var userId = User.FindFirstValue(SystemConstants.AppClaims.Id);
        command.SetUserId(userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("profile/update")]
    [Authorize]
    public IActionResult UpdateProfile()
    {
        return this.Ok(new { message = "API is under development" });
    }
    
    [HttpGet("profile")]
    [Authorize]
    public IActionResult GetMe()
    {
        return this.Ok(new { message = "API is under development" });
    }
    
    [HttpGet("profile/{id}")]
    public IActionResult GetProfileById(string id)
    {
        return this.Ok(new { message = "API is under development" });
    }
    
    [HttpPost("follow")]
    [SwaggerOperation(summary: "Follow user")]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiSuccessResult<string>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> FollowUser([FromBody] FollowUserCommand command)
    {
        var userId = User.FindFirstValue(SystemConstants.AppClaims.Id);
        command.SetFollowerId(userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete("unfollow")]
    [SwaggerOperation(summary: "Unfollow user")]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiSuccessResult<string>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> UnfollowUser([FromBody] UnfollowUserCommand command)
    {
        var userId = User.FindFirstValue(SystemConstants.AppClaims.Id);
        command.SetFollowerId(userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpGet("get-following")]
    [SwaggerOperation(summary: "Get Following current user")]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiSuccessResult<IEnumerable<User>>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetFollowing([FromQuery] GetFollowingQuery query)
    {
        var userId = User.FindFirstValue(SystemConstants.AppClaims.Id);
        query.SetFollowerId(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("get-followers")]
    [SwaggerOperation(summary: "Get Followers current user")]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiErrorResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiSuccessResult<IEnumerable<User>>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetFollowers([FromQuery] GetFollowersQuery query)
    {
        var userId = User.FindFirstValue(SystemConstants.AppClaims.Id);
        query.SetUserId(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}