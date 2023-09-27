using FluentValidation;

namespace Tiktok.API.Application.Features.Users.Commands.UnfollowUser;

public class UnfollowUserCommandValidator : AbstractValidator<UnfollowUserCommand>
{
    public UnfollowUserCommandValidator()
    {
        RuleFor(x=> x.UnfollowingId)
            .NotEmpty()
            .NotNull().WithMessage("FollowerId is required");
    }
}