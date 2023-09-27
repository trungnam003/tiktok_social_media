using AutoMapper;
using MediatR;
using Tiktok.API.Application.Common.DTOs;
using Tiktok.API.Application.Common.Mappers;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.TestBase.Queries.GetTestData;
#nullable disable
public class GetDataQuery : IRequest<ApiSuccessResult<TestDataDto>>, IMapFrom<TestDataDto>
{
    public int Age;
    public string Username;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TestDataDto, GetDataQuery>();
    }
}