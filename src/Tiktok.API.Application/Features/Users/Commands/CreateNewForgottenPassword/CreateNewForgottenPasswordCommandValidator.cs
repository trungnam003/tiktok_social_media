using FluentValidation;
using Tiktok.API.Application.Common.Validators;

namespace Tiktok.API.Application.Features.Users.Commands.CreateNewForgottenPassword;

public class CreateNewForgottenPasswordCommandValidator : AbstractValidator<CreateNewForgottenPasswordCommand>
{
    public CreateNewForgottenPasswordCommandValidator()
    {
        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("OTP is required")
            .NotNull()
            .Length(6).WithMessage("OTP must be 6 characters");
        
        RuleFor(x => x.NewPassword)
            .Password()
            .Equal(x => x.ConfirmNewPassword).WithMessage("NewPassword and ConfirmNewPassword must match");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
    }
}