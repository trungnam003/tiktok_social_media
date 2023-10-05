using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Exceptions;

namespace Tiktok.API.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, ApiSuccessResult<ProfileUserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserProfileHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ApiSuccessResult<ProfileUserDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userRequest = request.GetUserId();
        var isSelfRequest = request.IsSelf();
        var user = await _userRepository.GetUserByUserNameAsync(request.UserName);
        if (user == null)
        {
            throw new HttpException("User not found", StatusCodes.Status404NotFound);
        }

        var profileDto = new ProfileUserDto();
        profileDto.FollowingCount = await _userRepository.GetFollowingCountAsync(user.Id);
        profileDto.FollowerCount = await _userRepository.GetFollowersCountAsync(user.Id);
        
        if (string.IsNullOrEmpty(userRequest))
        {
            var result = _mapper.Map(user, profileDto);
            return new ApiSuccessResult<ProfileUserDto>(result);
        }

        if (isSelfRequest)
        {
            var result = _mapper.Map(user, profileDto);
            result.Self = isSelfRequest;
            return new ApiSuccessResult<ProfileUserDto>(result);
        }
        else
        {
            var result = _mapper.Map(user, profileDto);
            result.Self = isSelfRequest;
            var checkUserRequestFollowingUserTarget = await _userRepository.IsFollowingAsync(userRequest, user.Id);
            result.Following = checkUserRequestFollowingUserTarget;
            var checkUserTargetFollowingUserRequest = await _userRepository.IsFollowingAsync( user.Id, userRequest);
            result.Followed = checkUserTargetFollowingUserRequest;
            return new ApiSuccessResult<ProfileUserDto>(result);
        }
    }
}