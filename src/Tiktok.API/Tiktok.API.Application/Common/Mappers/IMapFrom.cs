using AutoMapper;

namespace Tiktok.API.Application.Common.Mappers;

public interface IMapFrom<T>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType());
    }
}