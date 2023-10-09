

using MediatR;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Exceptions;

namespace Tiktok.API.Application.Features.Comments.Commands.DeleteVideoComment;

public class DeleteVideoCommentHandler : IRequestHandler<DeleteVideoCommentCommand, ApiSuccessResult<string>>
{
    private readonly ICommentRepository _commentRepository;
    
    public DeleteVideoCommentHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }
    
    public async Task<ApiSuccessResult<string>> Handle(DeleteVideoCommentCommand request, CancellationToken cancellationToken)
    {
        var result = await _commentRepository.DeleteCommentAsync(request.CommentId, request.UserId);
        if (result)
        {
            return new ApiSuccessResult<string>("Delete comment successfully");
        }
        throw new BadRequestException("Delete comment failed or you don't have permission to delete this comment");
    }
}