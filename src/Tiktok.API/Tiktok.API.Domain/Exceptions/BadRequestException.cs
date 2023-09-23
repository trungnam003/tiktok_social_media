using Microsoft.AspNetCore.Http;

namespace Tiktok.API.Domain.Exceptions;

[Serializable]
public class BadRequestException : HttpException
{
    public BadRequestException(List<string> errors, int statusCode = StatusCodes.Status400BadRequest,
        string message = "API Error") : base(errors, statusCode, message)
    {
    }

    public BadRequestException(string error, int statusCode = StatusCodes.Status400BadRequest,
        string message = "API Error") : base(error, statusCode, message)
    {
    }
}