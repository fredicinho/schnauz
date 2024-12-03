using Schnauz.Shared.Interfaces;
using FluentValidation;
using Schnauz.Shared.Dtos;

namespace Schnauz.Shared.Commands;
public class ChangeCardCommand : ICommand
{
    public string Username { get; set; } = string.Empty;
    public CardDto CardInHand { get; set; } = null!;
    public CardDto CardOnTable { get; set; } = null!;
}

public class ChangeCardCommandValidator : FluentValidatorBase<ChangeCardCommand>
{
    public ChangeCardCommandValidator()
    {
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(40);
        RuleFor(x => x.CardInHand).NotNull();
        RuleFor(x => x.CardOnTable).NotNull();
    }
}
