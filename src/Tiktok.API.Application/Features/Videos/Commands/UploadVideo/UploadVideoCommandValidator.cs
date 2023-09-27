using FluentValidation;
using Tiktok.API.Application.Common.Validators;

namespace Tiktok.API.Application.Features.Videos.Commands.UploadVideo;

public class UploadVideoCommandValidator : AbstractValidator<UploadVideoCommand>
{
    public UploadVideoCommandValidator()
    {
        RuleFor(x => x.VideoUpload)
            .NotNull()
            .WithMessage("Video must not be null")
            .SetValidator(new FileValidator());
        RuleFor(x => x.Title)
            .MaximumLength(512)
            .WithMessage("Title must be less than 512 characters");
        
        RuleFor(x => x.UseExternalAudio)
            .NotNull()
            .Must(x => (x == false || x == true))
            .WithMessage("UseExternalAudio must be boolean");
        
        // validate array of tags
        RuleForEach(x => x.Tags)
            .NotEmpty().WithMessage("Tag must not be empty")
            .MaximumLength(128)
            .WithMessage("Tag must be less than 128 characters")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("Tag must only contain letters, numbers and underscores");
            
    }
}