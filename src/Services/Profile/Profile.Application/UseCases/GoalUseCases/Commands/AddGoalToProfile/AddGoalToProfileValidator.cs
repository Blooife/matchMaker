using FluentValidation;

namespace Profile.Application.UseCases.GoalUseCases.Commands.AddGoalToProfile;

public class AddGoalToProfileValidator : AbstractValidator<AddGoalToProfileCommand>
{
    public AddGoalToProfileValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        RuleFor(command => command.Dto.GoalId).NotEmpty().WithMessage("Goal id cant be empty");
    }
}