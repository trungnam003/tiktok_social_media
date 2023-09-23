using AutoMapper;
using Contracts.EventBusMessages.Events;
using Contracts.Services;
using MediatR;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Application.Features.Users.Commands.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ApiSuccessResult<RegisterResultDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPublishMessageService _publishMessageService;
    private readonly IOtpService _otpService;

    public RegisterUserHandler(IUserRepository userRepository, IMapper mapper,
        IPublishMessageService publishMessageService, IOtpService otpService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publishMessageService = publishMessageService;
        _otpService = otpService;
    }

    public async Task<ApiSuccessResult<RegisterResultDto>> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        await _userRepository.CreateUserAsync(user, request.Password);
        var result = _mapper.Map<RegisterResultDto>(user);
        var otp = await _otpService.GenerateOtpAsync(user.Email);
        var message = new SendMailForgotPasswordEvent()
        {
            Email = result.Email,
            FullName = user.FullName ?? result.Username,
            // random int from 100000 to 999999
            Otp = otp
        };
        await _publishMessageService.Publish(message, cancellationToken);
        return new ApiSuccessResult<RegisterResultDto>(result);
    }
}