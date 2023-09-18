using Microsoft.AspNetCore.Http;

namespace Tiktok.API.Domain.Exceptions;

[Serializable]
public class HttpException : Exception
{
    public HttpException(List<string> errors, int statusCode = StatusCodes.Status500InternalServerError,
        string message = "API Error") : base(message)
    {
        StatusCode = statusCode;
        Errors = errors;
    }

    public HttpException(string error, int statusCode = StatusCodes.Status500InternalServerError,
        string message = "API Error") : base(message)
    {
        StatusCode = statusCode;
        Errors = new List<string> { error };
    }

    public int StatusCode { get; }
    public List<string> Errors { get; }
}