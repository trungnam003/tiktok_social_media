using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Entities.Abstracts;

namespace Tiktok.API.Domain.Entities;
#nullable disable

[Table("Audios",Schema = SystemConstants.AppSchema)]
public class Audio : EntityAuditableBase<string>
{
    [Key]
    [Column(TypeName = "varchar(50)")] 
    public new string Id { get; set; } = Guid.NewGuid().ToString();

    [Column(TypeName = "varchar(255)")] 
    public string Url { get; set; }

    [Column(TypeName = "bigint")] 
    public long TotalUsed { get; set; }
    
    [Column(TypeName = "bigint")]
    public long MsDuration{ get; set; }
    
    [NotMapped]
    public virtual ICollection<Video> UsageVideos { get; set; }
    
    [NotMapped]
    public Video Source { get; set; }

    [Column(TypeName = "varchar(50)")]
    public string SourceId { get; set; }
    
    [NotMapped] 
    public User Owner { get; set; }
    
    [Column(TypeName = "varchar(50)")]
    [NotNull]
    public string OwnerId { get; set; }
}