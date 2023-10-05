using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.EventBusMessages.Events;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Application.Features.Users.Commands.UnfollowUser;

public class UnfollowUserHandler : IRequestHandler<UnfollowUserCommand, ApiSuccessResult<string>>
{
    
    private readonly IUserRepository _userRepository;
    public UnfollowUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiSuccessResult<string>> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
    {
        if(request.GetFollowerId().Equals(request.UnfollowingId))
            throw new HttpException("Can not unfollow yourself", StatusCodes.Status400BadRequest);
        var checkFollowerExist  = await _userRepository.GetUserByIdAsync(request.GetFollowerId());
        var checkFollowingExist  =  await _userRepository.GetUserByIdAsync(request.UnfollowingId);
        
        if (checkFollowerExist == null || checkFollowingExist == null)
            throw new HttpException("User not found", StatusCodes.Status404NotFound);
        var checkUserFollow = await _userRepository.CheckFollowUserAsync(checkFollowerExist, checkFollowingExist);
        if (!checkUserFollow)
            throw new HttpException("User not follow", StatusCodes.Status406NotAcceptable);

        var result = await _userRepository.UnfollowUserAsync(checkFollowerExist, checkFollowingExist);
        
        
        if (!result)
            throw new HttpException("Unfollow user failed");
        
        return new ApiSuccessResult<string>("Unfollow user successfully");
    }
}