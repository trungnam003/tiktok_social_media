
using AutoMapper;
using MediatR;
using Tiktok.API.Application.Common.DTOs.Comments;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.SeedWork;


namespace Tiktok.API.Application.Features.Comments.Queries.GetChildCommentOfRoot;

public class GetRootVideoCommentHandler : IRequestHandler<GetChildCommentOfRootQuery, ApiSuccessResult<IEnumerable<CommentDto>>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetRootVideoCommentHandler(ICommentRepository commentRepository, IUserRepository userRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ApiSuccessResult<IEnumerable<CommentDto>>> Handle(GetChildCommentOfRootQuery request, CancellationToken cancellationToken)
    {
        var childComments = await _commentRepository
            .GetChildCommentsByRootCommentIdAsync(request.GetCommentId(), request.GetVideoId(), request.PageNumber, request.PageSize);

        var count = await _commentRepository
            .CountChildCommentsByRootCommentIdAsync(request.GetCommentId(), request.GetVideoId());
        
        var listIdUser = childComments.Select(c => c.UserId).ToHashSet();
        var listIdReplyUser = childComments.Select(c => c.RelyUserId).ToHashSet();
        // merge 2 list
        listIdUser.UnionWith(listIdReplyUser);
        var listUser =  await _userRepository.GetUsersInListAsync(listIdUser, x => new User()
        {
            Id = x.Id,
            UserName = x.UserName,
            FullName = x.FullName,
            ImageUrl = x.ImageUrl
        });
        
        var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(childComments);
        for (var i = 0; i < childComments.Count(); i++)
        {
            var comment = childComments.ElementAt(i);
            var user = listUser.Where(x => x.Id.Equals(comment.UserId)).FirstOrDefault();
            var replyUser = listUser.Where(x => x.Id.Equals(comment.RelyUserId)).FirstOrDefault();
            commentsDto.ElementAt(i).User = _mapper.Map<UserDto>(user);
            commentsDto.ElementAt(i).UserReply = _mapper.Map<UserDto>(replyUser);
        }
        var pagination = new PagedList<CommentDto>(commentsDto, count, request.PageNumber, request.PageSize);
        return new ApiSuccessResult<IEnumerable<CommentDto>>(pagination, pagination.GetMetaData());

    }
}