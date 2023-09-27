using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Tiktok.API.Application.Common.Validators;

internal class FileValidator : AbstractValidator<IFormFile>
{
    public FileValidator()
    {
        // validate file less than 30mb
        RuleFor(x => x.Length)
            .GreaterThan(1*1024*1024)
            .WithMessage("File size must be greater than 1mb")
            .LessThan(30 * 1024 * 1024)
            .WithMessage("File size must be less than 30mb");
        // validate file type
        RuleFor(x => x.ContentType).Must(x => x.Equals("video/mp4"))
            .WithMessage("File type must be mp4");
    }
}