using FluentValidation;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Update;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileValidator()
    {
        RuleFor(command => command.Dto.BirthDate)
            .LessThanOrEqualTo(DateTime.Today.AddYears(-16))
            .WithMessage("Age must be at least 16 years.");
        RuleFor(command => command.Dto.MaxDistance)
            .NotEmpty().GreaterThanOrEqualTo(0)
            .WithMessage("Max distance must be >= 0");
        RuleFor(command => command.Dto.AgeFrom)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Age from must be >= 0");
        RuleFor(command => command.Dto.AgeTo)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Age to must be >= 0");
        RuleFor(command => command.Dto.AgeFrom)
            .LessThan(command => command.Dto.AgeTo)
            .WithMessage("Age from must be less than age to");
    }
}