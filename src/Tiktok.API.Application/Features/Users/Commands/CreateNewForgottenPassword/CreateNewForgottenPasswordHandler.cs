using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Application.Features.Users.Commands.CreateNewForgottenPassword;

public class CreateNewForgottenPasswordHandler : IRequestHandler<CreateNewForgottenPasswordCommand, ApiSuccessResult<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IOtpService _otpService;

    public CreateNewForgottenPasswordHandler(IUserRepository userRepository, IOtpService otpService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _otpService = otpService ?? throw new ArgumentNullException(nameof(otpService));
    }

    public async Task<ApiSuccessResult<string>> Handle(CreateNewForgottenPasswordCommand request, CancellationToken cancellationToken)
    {
        var verifyOtpResult = await _otpService.ValidateOtpAsync(request.Email, request.Otp);
        var result = verifyOtpResult switch
        {
            -1 => throw new BadRequestException("OTP does not match"),
            0 => throw new HttpException("OTP does not exist or expired", StatusCodes.Status410Gone),
            1 => true,
            _ => throw new ArgumentOutOfRangeException()
        };
        if (!result) throw new HttpException("OTP does not exist or expired", StatusCodes.Status410Gone);
        
        await _userRepository.ChangePasswordAsync(request.Email, request.NewPassword);
        await _otpService.DeleteOtpAsync(request.Email);
        return new ApiSuccessResult<string>("Password changed successfully");
    }
}