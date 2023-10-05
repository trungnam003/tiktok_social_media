using FluentValidation;

namespace Tiktok.API.Application.Features.Comments.Queries.GetRootVideoComment;

public class GetRootVideoCommentQueryValidator : AbstractValidator<GetRootVideoCommentQuery>
{
    public GetRootVideoCommentQueryValidator()
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