namespace Tiktok.API.Domain.Entities.Interfaces;

public interface IDateTracking
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}