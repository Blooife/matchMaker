using FluentValidation;

namespace Match.Application.UseCases.LikeUseCases.Commands.Create;

public class CreateLikeValidator : AbstractValidator<CreateLikeCommand>
{
    public CreateLikeValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty();
        RuleFor(command => command.Dto.TargetProfileId).NotEmpty();
        RuleFor(command => command.Dto.IsLike).NotEmpty();
    }
}