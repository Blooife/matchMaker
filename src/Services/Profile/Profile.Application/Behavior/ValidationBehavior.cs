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
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToList();

        if (validationResults.Any())
        {
            var errors = validationResults
                .Select(x => new ValidationError(x.PropertyName, x.ErrorMessage))
                .Distinct()
                .ToList();

            throw new Exceptions.ValidationException(
                errors
            );
        }

        return await next();
    }
}