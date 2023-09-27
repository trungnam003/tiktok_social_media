using FluentValidation;
using Tiktok.API.Application.Features.Users.Commands.UnfollowUser;

namespace Tiktok.API.Application.Features.Users.Commands.FollowUser;

public class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
{
    public FollowUserCommandValidator()
    {
        RuleFor(x=> x.FollowingId)
            .NotEmpty()
            .NotNull().WithMessage("FollowerId is required");
    }
}