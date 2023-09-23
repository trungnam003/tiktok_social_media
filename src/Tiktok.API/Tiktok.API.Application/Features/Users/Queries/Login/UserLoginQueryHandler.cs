using AutoMapper;
using Contracts.Services;
using MediatR;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Features.Users.Queries.Login;

public class UserLoginQueryHandler : IRequestHandler<UserLoginQuery, ApiSuccessResult<LoginResultDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IJwtService<User> _jwtService;

    public UserLoginQueryHandler(IUserRepository userRepository, IMapper mapper, IJwtService<User> jwtService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    public async Task<ApiSuccessResult<LoginResultDto>> Handle(UserLoginQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.LoginAsync(request.Email, request.Password);
            var token = await _jwtService.CreateToken(user);
            var result = _mapper.Map<LoginResultDto>(user);
            result.Token = token;
            return new ApiSuccessResult<LoginResultDto>(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}