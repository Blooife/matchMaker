using FluentValidation;
using MediatR;
using Profile.Application.Exceptions;

namespace Profile.Application.Behavior;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();
        
        var context = new ValidationContext<TRequest>(request);

        var validationResults = _validators
            .Select(val => val.Validate(context))
            .SelectMany(valResult => valResult.Errors)
            .Where(valFailure => valFailure is not null)
            .ToList();

        if (validationResults.Any())
        {
            var errors = validationResults
                .Select(valFailure => new ValidationError(valFailure.PropertyName, valFailure.ErrorMessage))
                .Distinct()
                .ToList();

            throw new Exceptions.ValidationException(
                errors
            );
        }

        return await next();
    }
}