using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Application.Features.Users.Queries.VerifyOtp;

public class VerifyOtpHandler : IRequestHandler<VerifyOtpQuery, ApiSuccessResult<string>>
{
    private readonly IOtpService _otpService;

    public VerifyOtpHandler(IOtpService otpService)
    {
        _otpService = otpService;
    }

    public async Task<ApiSuccessResult<string>> Handle(VerifyOtpQuery request, CancellationToken cancellationToken)
    {
        var validateResult = await _otpService.ValidateOtpAsync(request.Email, request.Otp);
        var result = validateResult switch
        {
            -1 => throw new BadRequestException("OTP does not match"),
            0 => throw new HttpException("OTP does not exist or expired", StatusCodes.Status410Gone),
            1 => "OTP is valid",
            _ => throw new ArgumentOutOfRangeException()
        };
        return new ApiSuccessResult<string>(result);
    }
}