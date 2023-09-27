using FluentValidation;
using Tiktok.API.Application.Common.Validators;

namespace Tiktok.API.Application.Features.Users.Commands.ChangePassword;

public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordCommandValidator()
    {
        RuleFor(x => x.OldPassword)
            .Password();

        RuleFor(x => x.NewPassword)
            .Password()
            .Equal(x => x.ConfirmNewPassword).WithMessage("Password and Confirm Password must match");
            
    }
}