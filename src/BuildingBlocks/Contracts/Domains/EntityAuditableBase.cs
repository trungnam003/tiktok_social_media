using Contracts.Domains.Interfaces;

namespace Contracts.Domains;

public class EntityAuditableBase<T> : EntityBase<T>, IDateTracking
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}