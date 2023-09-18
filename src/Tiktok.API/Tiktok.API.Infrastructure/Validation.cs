using FluentValidation;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Domain.Exceptions;

namespace Tiktok.API.Infrastructure;

public static class Validation
{
    public static async Task ValidateAsync<T, TK>(T validator, TK model)
        where T : AbstractValidator<TK>
    {
        var result = await validator.ValidateAsync(model);
        if (result.IsValid) return;
        var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
        throw new HttpException(errors, StatusCodes.Status400BadRequest);
    }
}