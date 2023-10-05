using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Domain.MongoEntities;

namespace Tiktok.API.Application.Features.Comments.Commands.AddCommentToVideo;

public class AddCommentToVideoHandler : IRequestHandler<AddCommentToVideoCommand, ApiSuccessResult<string>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IUserRepository _userRepository;

    public AddCommentToVideoHandler(ICommentRepository commentRepository, IVideoRepository videoRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _videoRepository = videoRepository;
        _userRepository = userRepository;
    }
    
    public async Task<ApiSuccessResult<string>> Handle(AddCommentToVideoCommand request, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetByIdAsync(request.VideoId);
        var user = await _userRepository.CheckUserExistAsync(x => x.Id.Equals(request.GetUserId()));
        
        if (video == null || !user)
        {
            throw new HttpException("Video or User not found", StatusCodes.Status404NotFound);
        }
        
        var comment = new Comment
        {
            Content = request.Comment,
            VideoId = request.VideoId,
            UserId = request.GetUserId(),
            IsRoot = string.IsNullOrEmpty(request.ReplyToCommentId)
        };
        
        if (!string.IsNullOrEmpty(request.ReplyToCommentId))
        {
            var commentReply = await _commentRepository.FindOneAsync(x=> x.Id.Equals(request.ReplyToCommentId));
            if (commentReply == null)
            {
                throw new HttpException("Comment not found", StatusCodes.Status404NotFound);
            }

            // update comment reply has child
            if (commentReply.IsRoot)
            {
                comment.RootId = commentReply.Id;
                comment.RelyUserId = null;
                if(!commentReply.HasChild)
                    await _commentRepository.SetHasChildAsync(commentReply.Id, true);
            }
            else
            {
                comment.RelyUserId = commentReply.UserId;
                comment.RootId = commentReply.RootId;
            }
        }
        
        
        
        await _commentRepository.AddAsync(comment);
        return new ApiSuccessResult<string>("Comment added successfully");
    }
}