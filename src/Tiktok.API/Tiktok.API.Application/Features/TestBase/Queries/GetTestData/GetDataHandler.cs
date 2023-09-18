using MediatR;
using Tiktok.API.Application.Common.DTOs;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.TestBase.Queries.GetTestData;

public class GetDataHandler : IRequestHandler<GetDataQuery, ApiSuccessResult<TestDataDto>>
{
    public Task<ApiSuccessResult<TestDataDto>> Handle(GetDataQuery request, CancellationToken cancellationToken)
    {
        var result = new TestDataDto
        {
            UserName = request.Username,
            Age = request.Age + 1
        };
        return Task.FromResult(new ApiSuccessResult<TestDataDto>(result));
    }
}