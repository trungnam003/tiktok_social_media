using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.EventBusMessages.Events;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Application.Features.Users.Commands.ForgotPassword;

public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, ApiSuccessResult<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IOtpService _otpService;
    private readonly IPublishMessageService _publishMessageService;

    public ForgotPasswordHandler(IUserRepository userRepository, IOtpService otpService, IPublishMessageService publishMessageService)
    {
        _userRepository = userRepository;
        _otpService = otpService;
        _publishMessageService = publishMessageService;
    }

    public async Task<ApiSuccessResult<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user == null) throw new HttpException("Email does not exist", StatusCodes.Status400BadRequest);
        var otp = await _otpService.GenerateOtpAsync(request.Email);
        var message = new SendMailForgotPasswordEvent()
        {
            Email = request.Email,
            Otp = otp,
            FullName = string.IsNullOrEmpty(user.FullName) ? user.UserName : user.FullName
        };
        await _publishMessageService.Publish(message, cancellationToken);
        return new ApiSuccessResult<string>("OTP has been sent to email");
    }
}