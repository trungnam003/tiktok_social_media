using AutoMapper;
using MediatR;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.EventBusMessages.Events;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Application.Features.Users.Commands.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ApiSuccessResult<RegisterResultDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RegisterUserHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ApiSuccessResult<RegisterResultDto>> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        await _userRepository.CreateUserAsync(user, request.Password);
        var result = _mapper.Map<RegisterResultDto>(user);
        return new ApiSuccessResult<RegisterResultDto>(result);
    }
}