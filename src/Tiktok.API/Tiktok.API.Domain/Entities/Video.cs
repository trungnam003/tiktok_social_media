using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Contracts.Domains;

namespace Tiktok.API.Domain.Entities;
#nullable disable

public class Video : EntityAuditableBase<string>
{
    [Column(TypeName = "varchar(50)")] public new string Id { get; set; } = Guid.NewGuid().ToString();

    [Column(TypeName = "varchar(1024)")] public string Title { get; set; }

    [Column(TypeName = "varchar(512)")] public string Url { get; set; }

    [Column(TypeName = "bigint")] public long TotalLove { get; set; }

    //not map
    [NotMapped] public User Owner { get; set; }

    [Column(TypeName = "varchar(50)")]
    [NotNull]
    public string OwnerId { get; set; }

    [NotMapped] public Audio? Audio { get; set; }

    [NotMapped] public Audio? ExternalAudio { get; set; }

    [Column(TypeName = "varchar(50)")] public string? ExternalAudioId { get; set; }
    
    [Column(TypeName = "bigint")]
    public long MsDuration{ get; set; }
    
    [Column(TypeName = "varchar(512)")]
    public string ThumbnailUrl { get; set; }
    
}