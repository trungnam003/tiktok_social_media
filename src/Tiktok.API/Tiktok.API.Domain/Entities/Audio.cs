using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Contracts.Domains;

namespace Tiktok.API.Domain.Entities;
#nullable disable
public class Audio : EntityAuditableBase<string>
{
    [Column(TypeName = "varchar(50)")] public new string Id { get; set; } = Guid.NewGuid().ToString();

    [Column(TypeName = "varchar(255)")] public string Url { get; set; }

    [Column(TypeName = "bigint")] public long TotalUsed { get; set; }
    
    [Column(TypeName = "bigint")]
    public long MsDuration{ get; set; }
    
    [NotMapped]
    public Video Source { get; set; }

    [Column(TypeName = "varchar(50)")][NotNull]
    public string SourceId { get; set; }
    [NotMapped]
    public ICollection<Video> UsageVideos { get; set; }
}