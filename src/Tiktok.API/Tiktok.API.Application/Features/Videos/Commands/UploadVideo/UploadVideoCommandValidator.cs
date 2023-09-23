using FluentValidation;
using Tiktok.API.Application.Common.Validators;

namespace Tiktok.API.Application.Features.Videos.Commands.UploadVideo;

public class UploadVideoCommandValidator : AbstractValidator<UploadVideoCommand>
{
    public UploadVideoCommandValidator()
    {
        RuleFor(x => x.VideoUpload)
            .SetValidator(new FileValidator());
        RuleFor(x => x.Title)
            .MaximumLength(512)
            .WithMessage("Title must be less than 512 characters");
        
        RuleFor(x => x.UseExternalAudio)
            .NotNull()
            .Must(x => (x == false || x == true))
            .WithMessage("UseExternalAudio must be boolean");
            
            
            
    }
}