using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Entities.Abstracts;

namespace Tiktok.API.Domain.Entities;
#nullable disable
[Table("Videos", Schema = SystemConstants.AppSchema)]
public class Video : EntityAuditableBase<string>
{
    
    [Key][Column(TypeName = "varchar(50)")] 
    public new string Id { get; set; } = Guid.NewGuid().ToString();

    [Column(TypeName = "nvarchar(1024)")] 
    public string Title { get; set; }

    [Column(TypeName = "varchar(512)")] 
    public string Url { get; set; }
    
    [NotMapped] 
    public User Owner { get; set; }

    [Column(TypeName = "varchar(50)")]
    [NotNull]
    public string OwnerId { get; set; }

    [NotMapped] 
    public Audio? Audio { get; set; }

    [NotMapped] 
    public Audio? ExternalAudio { get; set; }

    [Column(TypeName = "varchar(50)")] 
    public string? ExternalAudioId { get; set; }
    
    [Column(TypeName = "bigint")]
    public long MsDuration{ get; set; }
    
    [Column(TypeName = "varchar(512)")]
    public string ThumbnailUrl { get; set; }
    
    [Column(TypeName = "varchar(512)")]
    public string? Tags { get; set; }
    
}