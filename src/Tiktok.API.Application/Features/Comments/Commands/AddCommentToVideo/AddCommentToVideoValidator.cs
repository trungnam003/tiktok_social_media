using FluentValidation;

namespace Tiktok.API.Application.Features.Comments.Commands.AddCommentToVideo;

public class AddVideoToCommentValidator : AbstractValidator<AddCommentToVideoCommand>
{
    public AddVideoToCommentValidator()
    {
        RuleFor(x => x.VideoId)
            .NotEmpty().WithMessage("VideoId is required.")
            .NotNull();

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .NotNull()
            .MaximumLength(512).WithMessage("Comment must not exceed 512 characters.");
    }
}