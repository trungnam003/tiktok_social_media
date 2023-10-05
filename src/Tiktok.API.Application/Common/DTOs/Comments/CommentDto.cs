using AutoMapper;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Mappers;
using Tiktok.API.Domain.MongoEntities;

namespace Tiktok.API.Application.Common.DTOs.Comments;

public class CommentDto : IMapFrom<Comment>
{
    public string Id { get; set; } = null!;
    public string Content { get; set; } = null!;
    
    public string VideoId { get; set; } = null!;
    
    public UserDto User { get; set; } = null!;
    
    public bool IsRoot { get; set; }
    
    public string? RootId { get; set; }
    
    public UserDto UserReply { get; set; } = null!;
    
    public int ReactionCount { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public int CountChild { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(CommentDto), typeof(Comment)).ReverseMap();
    }
}