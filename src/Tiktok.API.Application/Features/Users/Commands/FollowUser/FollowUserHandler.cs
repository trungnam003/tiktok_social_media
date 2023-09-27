using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Exceptions;

namespace Tiktok.API.Application.Features.Users.Commands.FollowUser;

public class FollowUserHandler : IRequestHandler<FollowUserCommand, ApiSuccessResult<string>>
{
    private readonly IUserRepository _userRepository;

    public FollowUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiSuccessResult<string>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        if(request.GetFollowerId().Equals(request.FollowingId))
            throw new HttpException("Can not follow yourself", StatusCodes.Status400BadRequest);
        var checkFollowerExist  = await _userRepository.GetUserByIdAsync(request.GetFollowerId());
        var checkFollowingExist  =  await _userRepository.GetUserByIdAsync(request.FollowingId);
        
        if (checkFollowerExist == null || checkFollowingExist == null)
            throw new HttpException("User not found", StatusCodes.Status404NotFound);

        var result = await _userRepository.FollowUserAsync(checkFollowerExist, checkFollowingExist);
        if (!result)
            throw new HttpException("Follow user failed");

        return new ApiSuccessResult<string>("Follow user successfully");
    }
}