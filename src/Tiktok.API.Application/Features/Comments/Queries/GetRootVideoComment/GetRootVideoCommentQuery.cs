using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tiktok.API.Application.Common.DTOs.Comments;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.SeedWork;

namespace Tiktok.API.Application.Features.Comments.Queries.GetRootVideoComment;

public class GetRootVideoCommentQuery : PagingRequestParameters, IRequest<ApiSuccessResult<IEnumerable<CommentDto>>>
{
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
    [FromQuery(Name = "orderBy")]
    public override string? OrderBy
    {
        get => _orderBy == 1 ? "asc" : "desc";
        set => _orderBy = value == "asc" ? (byte) 1 : (byte) 2;
    }
    
    private string _videoId = null!;
    public void SetVideoId(string videoId)
    {
        this._videoId = videoId;
    }
    public string GetVideoId()
    {
        return this._videoId;
    }
}