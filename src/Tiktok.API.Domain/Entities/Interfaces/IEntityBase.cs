namespace Tiktok.API.Domain.Entities.Interfaces;

public interface IEntityBase<T>
{
    T Id { get; set; }
}