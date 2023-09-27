namespace Tiktok.API.Domain.Entities.Interfaces;

public interface IMongoDateTracking
{
    public DateTime CreatedDate { get; set; }
    
    public DateTime? LastModifiedDate { get; set; }
}