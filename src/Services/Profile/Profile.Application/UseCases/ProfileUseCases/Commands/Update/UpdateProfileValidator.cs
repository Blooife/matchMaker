using FluentValidation;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Update;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileValidator()
    {
        RuleFor(command => command.UpdateProfileDto.Name)
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage("Name must be greater than 2 and less than 50 characters");
        
        RuleFor(command => command.UpdateProfileDto.LastName)
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage("Lastname must be greater than 2 and less than 50 characters");
        
        RuleFor(command => command.UpdateProfileDto.Bio)
            .MinimumLength(10)
            .MaximumLength(500)
            .WithMessage("Bio must be greater than 50 and less than 500 characters");
        
        RuleFor(command => command.UpdateProfileDto.Height)
            .GreaterThanOrEqualTo(100)
            .LessThanOrEqualTo(220)
            .WithMessage("Height must be greater than 100 and less than 220");
        
        RuleFor(command => command.UpdateProfileDto.BirthDate)
            .LessThanOrEqualTo(DateTime.Today.AddYears(-16))
            .WithMessage("Age must be at least 16 years.");
        
        RuleFor(command => command.UpdateProfileDto.MaxDistance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Max distance must be >= 0");
        
        RuleFor(command => command.UpdateProfileDto.AgeFrom)
            .GreaterThanOrEqualTo(16)
            .WithMessage("Age from must be >= 16");
        
        RuleFor(command => command.UpdateProfileDto.AgeTo)
            .GreaterThanOrEqualTo(16)
            .WithMessage("Age to must be >= 16");
        
        RuleFor(command => command.UpdateProfileDto.AgeFrom)
            .LessThanOrEqualTo(command => command.UpdateProfileDto.AgeTo)
            .WithMessage("Age from must be less than or equal age to");
    }
}