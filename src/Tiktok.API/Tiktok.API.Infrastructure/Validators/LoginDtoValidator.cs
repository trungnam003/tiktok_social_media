using FluentValidation;
using Tiktok.API.Application.Common.DTOs.Users;

namespace Tiktok.API.Infrastructure.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress();
        RuleFor(x => x.Password)
            .MinimumLength(3)
            .NotEmpty();
    }
}