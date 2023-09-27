using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.SeedWork;

namespace Tiktok.API.Application.Features.Users.Queries.GetFollowingWithPaging;
#nullable disable
public class GetFollowingQuery : PagingRequestParameters, IRequest<ApiSuccessResult<IEnumerable<UserDto>>>
{
    [FromQuery(Name = "q")]
    public string Q { get; set; }

    [FromQuery(Name = "pageNumber")]
    public override int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }
    [FromQuery(Name = "pageSize")]
    public override int PageSize
    {
        get => _pageSize;
        set
        {
            if (value > 0)
                _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
    // disable swagger
    public override string? OrderBy
    {
        get => _orderBy == 1 ? "asc" : "desc";
        set => _orderBy = value == "asc" ? (byte) 1 : (byte) 2;
    }
    
    private string _followerId;
    public void SetFollowerId(string follower)
    {
        _followerId = follower;
    }
    
    public string GetFollowerId()
    {
        return _followerId;
    }
}