using FluentValidation;

namespace Profile.Application.UseCases.GoalUseCases.Commands.RemoveGoalFromProfile;

public class RemoveGoalFromProfileValidator : AbstractValidator<RemoveGoalFromProfileCommand>
{
    public RemoveGoalFromProfileValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
    }
}