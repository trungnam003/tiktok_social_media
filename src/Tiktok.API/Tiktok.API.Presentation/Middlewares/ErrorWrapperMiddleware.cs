using System.Net;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Exceptions;

namespace Tiktok.API.Presentation.Middlewares;

public class ErrorWrapperMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            if (e is HttpException httpException)
            {
                context.Response.StatusCode = httpException.StatusCode;
                var result = new ApiErrorResult(httpException.Errors);
                await context.Response.WriteAsJsonAsync(result);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var result = new ApiErrorResult(e.Message);
                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}