using FluentValidation;

namespace Tiktok.API.Application.Features.Users.Queries.VerifyOtp;

public class VerifyOtpQueryValidator : AbstractValidator<VerifyOtpQuery>
{
    public VerifyOtpQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
        
        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("Email is required")
            .Must(x => x.Length == 6).WithMessage("OTP must be 6 digits");
    }
}