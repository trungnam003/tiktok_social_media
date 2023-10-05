using Newtonsoft.Json;
using Tiktok.API.Domain.SeedWork;

namespace Tiktok.API.Domain.Common.Models;

public class ApiSuccessResult<T> : ApiResult
{
    [JsonConstructor]
    public ApiSuccessResult(T data) : base(true)
    {
        Data = data;
    }
    
    public ApiSuccessResult(T data, MetaData metaData) : base(true)
    {
        Data = data;
        MetaData = metaData;
    }
    public MetaData? MetaData { get; set; }
    public T Data { get; set; }
}