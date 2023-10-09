using FluentValidation;

namespace Tiktok.API.Application.Features.Comments.Commands.DeleteVideoComment;
#nullable disable
public class DeleteVideoCommentCommandValidator : AbstractValidator<DeleteVideoCommentCommand>
{
    public DeleteVideoCommentCommandValidator()
    {
        RuleFor(x => x.CommentId)
            .NotNull()
            .NotEmpty()
            .WithMessage("CommentId is required");
        
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty()
            .WithMessage("CommentId is required");
    }
}