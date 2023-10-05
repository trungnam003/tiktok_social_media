using MediatR;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Comments.Commands.AddCommentToVideo;
#nullable disable
public class AddCommentToVideoCommand : IRequest<ApiSuccessResult<string>>
{
    public string VideoId { get; set; }
    public string Comment { get; set; }
    public string ReplyToCommentId { get; set; }
    
    private string _userId;
    public void SetUserId(string userId)
    {
        _userId = userId;
    }
    
    public string GetUserId()
    {
        return _userId;
    }
}