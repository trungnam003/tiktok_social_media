using AutoMapper;
using MediatR;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.SeedWork;

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
        var user = await _userRepository.GetUserByIdAsync(request.GetFollowerId());
        var users = await _userRepository
            .GetFollowingWithPagingAsync(request.GetFollowerId(), request.PageNumber, request.PageSize, request.Q);
        var result = _mapper.Map<IEnumerable<UserDto>>(users);
        var followingCount = await _userRepository.GetFollowingCountAsync(user.Id);
        var items = new PagedList<UserDto>(result, followingCount, request.PageNumber, request.PageSize);
        
        return new ApiSuccessResult<IEnumerable<UserDto>>(items, items.GetMetaData());
    }
}