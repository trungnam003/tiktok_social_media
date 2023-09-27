using MediatR;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Users.Queries.VerifyOtp;
#nullable disable
public class VerifyOtpQuery : IRequest<ApiSuccessResult<string>>
{
    public string Otp { get; set; }
    public string Email { get; set; }
}