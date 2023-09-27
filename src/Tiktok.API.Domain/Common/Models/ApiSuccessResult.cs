using Newtonsoft.Json;

namespace Tiktok.API.Domain.Common.Models;

public class ApiSuccessResult<T> : ApiResult
{
    [JsonConstructor]
    public ApiSuccessResult(T data) : base(true)
    {
        Data = data;
    }

    public T Data { get; set; }
}