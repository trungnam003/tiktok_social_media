using FluentValidation;
using Tiktok.API.Application.Common.Validators;

namespace Tiktok.API.Application.Features.Users.Queries.Login;

public class UserLoginValidator : AbstractValidator<UserLoginQuery>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .Password();
    }
}