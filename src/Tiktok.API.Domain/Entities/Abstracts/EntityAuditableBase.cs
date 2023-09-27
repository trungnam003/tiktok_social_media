using Tiktok.API.Domain.Entities.Interfaces;
namespace Tiktok.API.Domain.Entities.Abstracts;

public class EntityAuditableBase<T> : EntityBase<T>, IDateTracking
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}