using AutoMapper;
using MediatR;
using MongoDB.Driver.Linq;
using Tiktok.API.Application.Common.DTOs.Comments;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.SeedWork;

namespace Tiktok.API.Application.Features.Comments.Queries.GetRootVideoComment;

public class GetRootVideoCommentHandler : IRequestHandler<GetRootVideoCommentQuery, ApiSuccessResult<IEnumerable<CommentDto>>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetRootVideoCommentHandler(ICommentRepository commentRepository, IMapper mapper, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }
    
    public async Task<ApiSuccessResult<IEnumerable<CommentDto>>> Handle(GetRootVideoCommentQuery request, CancellationToken cancellationToken)
    {

        var comments = await _commentRepository
            .GetRootCommentsByVideoIdAsync(request.GetVideoId(), request.PageNumber, request.PageSize);
        
        var count = await _commentRepository
            .CountRootCommentsByVideoIdAsync(request.GetVideoId());
        var listIdUser = comments.Select(c => c.UserId).ToHashSet();
        
        var listUser = await _userRepository.GetUsersInListAsync(listIdUser, x => new User()
        {
            Id = x.Id,
            UserName = x.UserName,
            FullName = x.FullName,
            ImageUrl = x.ImageUrl
        });
        
        var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
        for (var i = 0; i < comments.Count(); i++)
        {
            var comment = comments.ElementAt(i);
            var user = listUser.Where(x => x.Id.Equals(comment.UserId)).FirstOrDefault();
            commentsDto.ElementAt(i).User = _mapper.Map<UserDto>(user);
        }
        var pagination = new PagedList<CommentDto>(commentsDto, count, request.PageNumber, request.PageSize);
        
        return new ApiSuccessResult<IEnumerable<CommentDto>>(pagination, pagination.GetMetaData());
    }
}