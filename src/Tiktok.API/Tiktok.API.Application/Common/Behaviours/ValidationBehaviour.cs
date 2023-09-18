using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Domain.Exceptions;

namespace Tiktok.API.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var tasks = _validators.Select(v => v.ValidateAsync(context, cancellationToken));
        var validationResults = await Task.WhenAll(tasks);
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
        var failureMessages = failures.Select(f => f.ErrorMessage).ToList();
        if (failures.Any())
            throw new HttpException(failureMessages, StatusCodes.Status400BadRequest);

        return await next();
    }
}