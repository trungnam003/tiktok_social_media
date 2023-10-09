using MediatR;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Comments.Commands.DeleteVideoComment;
#nullable disable
public class DeleteVideoCommentCommand : IRequest<ApiSuccessResult<string>>
{
    public string CommentId { get; set; }
    public string UserId { get; set; }
}