using System.Net;
using AutoMapper;
using Contracts.EventBusMessages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Tiktok.API.Application.Common.DTOs;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Application.Features.TestBase.Queries.GetTestData;
using Tiktok.API.Domain.MongoEntities;
using Tiktok.API.Infrastructure.Helper;
using Tiktok.API.Presentation.Attributes;

namespace Tiktok.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IHostEnvironment _hostingEnvironment;
    private readonly IPublishEndpoint _publish;
    private readonly IVideoTagRepository _videoTagRepository;

    public TestController(IMediator mediator, IMapper mapper, IHostEnvironment hostingEnvironment,
        IPublishEndpoint publish, IVideoTagRepository videoTagRepository)
    {
        _mediator = mediator;
        _mapper = mapper;
        _hostingEnvironment = hostingEnvironment;
        _publish = publish;
        _videoTagRepository = videoTagRepository;
    }

    [HttpPost("increase-video-tag")]
    public async Task<ActionResult> TestAddVideoTag(string tagName)
    {
        await _videoTagRepository.IncreaseTagViewCount(tagName);
        return Ok();
    }
    
    [HttpGet("publish-send-email")]
    public async Task<ActionResult> Get()
    {
        var message = new SendMailForgotPasswordEvent()
        {
            Email = "zztrungnamzz@gmail.com",
            FullName = "Trung Nam",
            Otp = "123456"
        };
        await _publish.Publish(message);
        return Ok("Success");
    }

    [HttpGet("get-data-test")]
    public async Task<ActionResult> GetDataTest([FromQuery] TestDataDto data)
    {
        var query = _mapper.Map<GetDataQuery>(data);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    private const long MaxFileSize = 80L * 1024L * 1024L;


    [HttpPost("upload-file")]
    [Consumes("multipart/form-data")]
    [DisableFormValueModelBinding]
    [RequestSizeLimit(MaxFileSize)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    public async Task<ActionResult> UploadFile()
    {
        try
        {
            var request = HttpContext.Request;
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File",
                    $"The request couldn't be processed (Error 1).");
                return BadRequest(ModelState);
            }

            // var form = await request.ReadFormAsync(); // async

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType));
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (!MultipartRequestHelper
                            .HasFileContentDisposition(contentDisposition))
                    {
                        section = await reader.ReadNextSectionAsync();
                        continue;
                    }

                    var trustedFileNameForFileStorage = Guid.NewGuid().ToString() + contentDisposition.FileName.Value;

                    var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                        section, contentDisposition, ModelState,
                        new[] { ".mp4" }, MaxFileSize);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var projectRootPath = _hostingEnvironment.ContentRootPath.TrimEnd('\\').TrimEnd('/');
                    var storagePath =
                        Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(projectRootPath)));
                    if (storagePath == null) return BadRequest("Error");
                    var targetFilePath = Path.Combine(storagePath, "storage", "videos");
                    await using var targetStream =
                        System.IO.File.Create(Path.Combine(targetFilePath, trustedFileNameForFileStorage));
                    await targetStream.WriteAsync(streamedFileContent);
                }

                section = await reader.ReadNextSectionAsync();
            }

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("upload-file-2")]
    [Consumes("multipart/form-data")]
    // conten type mp4 requrie
    [RequestSizeLimit(MaxFileSize)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    public async Task<ActionResult> UploadFile2([FromForm] Test test)
    {
        var file = test.File;
        var projectRootPath = _hostingEnvironment.ContentRootPath.TrimEnd('\\').TrimEnd('/');
        var storagePath =
            Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(projectRootPath)));
        var fileData = file.FileName;
        await using var f = file.OpenReadStream();
        var isValid = FileHelpers.IsValidFileExtensionAndSignature(fileData, f, new[] { ".mp4" });
        if (!isValid) return BadRequest();
        var targetFilePath = Path.Combine(storagePath, "storage", "videos");
        await using var targetStream =
            System.IO.File.Create(Path.Combine(targetFilePath, fileData));
        await file.CopyToAsync(targetStream);
        await f.DisposeAsync();
        await targetStream.DisposeAsync();
        return Ok(fileData);
    }

    public class Test
    {
        [FromForm(Name = "json")] public string Json { get; set; }
        [FromForm(Name = "file")] public IFormFile File { get; set; }
    }
}