using Microsoft.AspNetCore.Http;

namespace Tiktok.API.Domain.Exceptions;

public class UnauthorizedException : HttpException
{
    public UnauthorizedException(List<string> errors, int statusCode = StatusCodes.Status401Unauthorized,
        string message = "API Error") : base(errors, statusCode, message)
    {
    }

    public UnauthorizedException(string error, int statusCode = StatusCodes.Status401Unauthorized,
        string message = "API Error") : base(error, statusCode, message)
    {
    }
}