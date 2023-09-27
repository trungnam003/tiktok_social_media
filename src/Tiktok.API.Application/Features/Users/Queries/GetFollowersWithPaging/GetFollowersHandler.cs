using AutoMapper;
using MediatR;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;

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
        var users = await _userRepository
            .GetFollowersWithPagingAsync(request.GetUserId(), request.PageNumber, request.PageSize);
        var result = _mapper.Map<IEnumerable<UserDto>>(users);
        return new ApiSuccessResult<IEnumerable<UserDto>>(result);
    }
}