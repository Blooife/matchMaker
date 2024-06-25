using FluentValidation;

namespace Match.Application.UseCases.ChatUseCases.Commands.Create;

public class CreateChatValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatValidator()
    {
        RuleFor(command => command.Dto.ProfileId1).NotEmpty();
        RuleFor(command => command.Dto.ProfileId2).NotEmpty();
    }
}