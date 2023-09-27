using Newtonsoft.Json;

namespace Tiktok.API.Domain.Common.Models;

public class ApiErrorResult : ApiResult
{
    [JsonConstructor]
    public ApiErrorResult(IList<string> errorMessages) : base(false)
    {
        ErrorMessages = errorMessages;
    }

    public ApiErrorResult(string errorMessage) : base(false)
    {
        ErrorMessages = new List<string> { errorMessage };
    }

    public IList<string> ErrorMessages { get; set; }
}