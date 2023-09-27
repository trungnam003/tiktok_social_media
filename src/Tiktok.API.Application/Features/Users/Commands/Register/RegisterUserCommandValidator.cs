using FluentValidation;
using Tiktok.API.Application.Common.Validators;

namespace Tiktok.API.Application.Features.Users.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .NotNull().WithMessage("Username is required")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters")
            .MaximumLength(255).WithMessage("Username must not exceed 255 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull().WithMessage("Email is required")
            .EmailAddress();

        RuleFor(x => x.Password)
            .Password()
            .Equal(x => x.ConfirmPassword).WithMessage("Password and Confirm Password must match");
    }
}