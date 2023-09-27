using Newtonsoft.Json;

namespace Tiktok.API.Domain.Common.Models;

public class ApiResult
{
    [JsonConstructor]
    public ApiResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public bool IsSuccess { get; set; }
}