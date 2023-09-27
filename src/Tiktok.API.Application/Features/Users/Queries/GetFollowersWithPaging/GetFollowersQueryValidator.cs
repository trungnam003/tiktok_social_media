using FluentValidation;

namespace Tiktok.API.Application.Features.Users.Queries.GetFollowersWithPaging;

public class GetFollowersQueryValidator :AbstractValidator<GetFollowersQuery>
{
    public GetFollowersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal 1");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize must be greater than or equal 1");
    }
}