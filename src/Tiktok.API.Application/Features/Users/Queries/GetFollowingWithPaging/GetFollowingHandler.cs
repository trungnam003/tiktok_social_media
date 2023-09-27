using AutoMapper;
using MediatR;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Features.Users.Queries.GetFollowingWithPaging;

public class GetFollowingHandler : IRequestHandler<GetFollowingQuery, ApiSuccessResult<IEnumerable<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetFollowingHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ApiSuccessResult<IEnumerable<UserDto>>> Handle(GetFollowingQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository
            .GetFollowingWithPagingAsync(request.GetFollowerId(), request.PageNumber, request.PageSize, request.Q);
        var result = _mapper.Map<IEnumerable<UserDto>>(users);
        return new ApiSuccessResult<IEnumerable<UserDto>>(result);
    }
}