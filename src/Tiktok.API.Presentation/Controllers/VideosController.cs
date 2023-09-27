using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tiktok.API.Application.Features.Videos.Commands.UploadVideo;
using Tiktok.API.Application.Features.Videos.Queries.GetVideoById;
using Tiktok.API.Application.Features.Videos.Queries.StreamVideoById;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Exceptions;

namespace Tiktok.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideosController : ControllerBase
{
    private readonly IMediator _mediator;
    private const long MaxFileSize = 80L * 1024L * 1024L;

    public VideosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    
    [HttpPost]
    [Authorize]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(MaxFileSize)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    public async Task<IActionResult> UploadVideo([FromForm] UploadVideoCommand model)
    {
        // get claims required
        var userId = User.FindFirstValue(SystemConstants.AppClaims.Id);
        if (userId == null) throw new ForbiddenException("Request is not authorized");

        model.SetOwner(userId);

        await _mediator.Send(model);
        return Ok();
    }
    
    [HttpGet("{id}/stream")]
    public async Task<IActionResult> StreamVideoById([Required][FromRoute]string id)
    {
        var query = new StreamVideoByIdQuery()
        {
            Id = id
        };
        var path = await _mediator.Send(query);
        
        return PhysicalFile(path, "video/mp4", enableRangeProcessing: true);
    }

    
    [HttpGet("{id}/info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    public async Task<IActionResult> GetVideoInfoById([Required][FromRoute]string id)
    {
        var user = User.Identity.IsAuthenticated;
        var query = new GetVideoByIdQuery()
        {
            Id = id
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}