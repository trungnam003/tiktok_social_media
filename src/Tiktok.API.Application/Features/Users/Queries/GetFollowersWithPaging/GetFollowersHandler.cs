using AutoMapper;
using MediatR;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.SeedWork;

namespace Tiktok.API.Application.Features.Users.Queries.GetFollowersWithPaging;

public class GetFollowersHandler : IRequestHandler<GetFollowersQuery, ApiSuccessResult<IEnumerable<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetFollowersHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ApiSuccessResult<IEnumerable<UserDto>>> Handle(GetFollowersQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.GetUserId());
        var users = await _userRepository
            .GetFollowersWithPagingAsync(request.GetUserId(), request.PageNumber, request.PageSize);
        var result = _mapper.Map<IEnumerable<UserDto>>(users);
        var followerCount = await _userRepository.GetFollowersCountAsync(user.Id);
        var items = new PagedList<UserDto>(result, followerCount, request.PageNumber, request.PageSize);

        return new ApiSuccessResult<IEnumerable<UserDto>>(items, items.GetMetaData());
    }
}