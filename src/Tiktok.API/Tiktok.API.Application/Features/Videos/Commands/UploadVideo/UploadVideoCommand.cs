using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Videos.Commands.UploadVideo;
#nullable disable
public class UploadVideoCommand : IRequest<ApiSuccessResult<string>>
{
    [FromForm(Name = "title")] public string Title { get; set; }
    [FromForm(Name = "video")] public IFormFile VideoUpload { get; set; }
    
    [FromForm(Name = "externalAudio")] public string ExternalAudioId { get; set; }
    
    [FromForm(Name = "useExternalAudio")] public bool UseExternalAudio { get; set; }

    private string _ownerId;

    public void SetOwner(string ownerId)
    {
        _ownerId = ownerId;
    }

    public string GetOwner()
    {
        return _ownerId;
    }
}