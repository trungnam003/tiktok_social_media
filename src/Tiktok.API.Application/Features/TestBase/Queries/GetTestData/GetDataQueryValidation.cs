using FluentValidation;

namespace Tiktok.API.Application.Features.TestBase.Queries.GetTestData;

public class GetDataQueryValidation : AbstractValidator<GetDataQuery>
{
    public GetDataQueryValidation()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage(x => $"{nameof(x.Username)} is required");
        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18).WithMessage("{1} must be greater than or equal to 18")
            .NotEmpty().WithMessage("{1} is required");
    }
}