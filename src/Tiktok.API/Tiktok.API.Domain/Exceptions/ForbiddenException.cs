using Microsoft.AspNetCore.Http;

namespace Tiktok.API.Domain.Exceptions;

[Serializable]
public class ForbiddenException : HttpException
{
    public ForbiddenException(List<string> errors, int statusCode = StatusCodes.Status403Forbidden,
        string message = "API Error") : base(errors, statusCode, message)
    {
    }

    public ForbiddenException(string error, int statusCode = StatusCodes.Status403Forbidden,
        string message = "API Error") : base(error, statusCode, message)
    {
    }
}