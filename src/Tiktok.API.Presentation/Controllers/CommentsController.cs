using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tiktok.API.Application.Features.Comments.Commands.AddCommentToVideo;
using Tiktok.API.Application.Features.Comments.Queries.GetChildCommentOfRoot;
using Tiktok.API.Application.Features.Comments.Queries.GetRootVideoComment;
using Tiktok.API.Domain.Common.Constants;

namespace Tiktok.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateComment([FromBody] AddCommentToVideoCommand command)
    {
        var userId = User.FindFirstValue(SystemConstants.AppClaims.Id);
        command.SetUserId(userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpGet("{videoId}")]
    public async Task<IActionResult> GetComments([FromRoute] string videoId, [FromQuery] GetRootVideoCommentQuery query)
    {
        query.SetVideoId(videoId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("{videoId}/{commentId}")]
    public async Task<IActionResult> GetChildComments([FromRoute] string videoId, [FromRoute] string commentId, [FromQuery] GetChildCommentOfRootQuery query)
    {
        query.SetVideoId(videoId);
        query.SetCommentId(commentId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}