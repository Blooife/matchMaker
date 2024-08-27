using FluentValidation;

namespace Match.Application.UseCases.LikeUseCases.Commands.Add;

public class AddLikeValidator : AbstractValidator<AddLikeCommand>
{
    public AddLikeValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty();
        RuleFor(command => command.Dto.TargetProfileId).NotEmpty();
    }
}