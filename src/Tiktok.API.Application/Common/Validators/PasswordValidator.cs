using FluentValidation;

namespace Tiktok.API.Application.Common.Validators;

public static class PasswordValidatorExtension
{
    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        var options = ruleBuilder
            .NotEmpty()
            .NotNull().WithMessage("Password is required")
            .MinimumLength(4).WithMessage("Password must be at least 4 characters")
            .MaximumLength(255).WithMessage("Password must not exceed 255 characters")
            .Matches(@"^(?=.*[A-Za-z\d])(?=.*\d)[A-Za-z\d@$!%*#?&]{4,255}$")
            .WithMessage("Password must contain at least one letter and one number");

        return options;
    }
}