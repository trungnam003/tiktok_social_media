using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Tiktok.API.Application.Features.Videos.Commands.DeleteVideo;
#nullable disable
public class DeleteVideoCommand : IRequest<bool>
{
    [FromRoute(Name = "id")]
    public string Id { get; set; }
    
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