using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tiktok.API.Application.Features.Videos.Commands.UploadVideo;
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
}