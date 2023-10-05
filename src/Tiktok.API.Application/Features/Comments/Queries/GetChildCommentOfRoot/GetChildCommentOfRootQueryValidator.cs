using FluentValidation;

namespace Tiktok.API.Application.Features.Comments.Queries.GetChildCommentOfRoot;

public class GetChildCommentOfRootQueryValidator : AbstractValidator<GetChildCommentOfRootQuery>
{
    public GetChildCommentOfRootQueryValidator()
    {
        RuleFor(x => x.GetVideoId())
            .NotEmpty()
            .NotNull();
        
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal 1");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize must be greater than or equal 1");
    }
}