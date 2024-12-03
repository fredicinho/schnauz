using Schnauz.Shared.Interfaces;
using FluentValidation;

namespace Schnauz.Shared.Commands;
public class ChangeAllCardsCommand : ICommand
{
    public string Username { get; set; } = string.Empty;
}

public class ChangeAllCardsCommandValidator : FluentValidatorBase<ChangeAllCardsCommand>
{
    public ChangeAllCardsCommandValidator()
    {
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(40);
    }
}
