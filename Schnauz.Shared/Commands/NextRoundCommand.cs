using Schnauz.Shared.Interfaces;
using FluentValidation;

namespace Schnauz.Shared.Commands;
public class NextRoundCommand : ICommand
{
    public string Username { get; set; } = string.Empty;
}

public class NextRoundCommandValidator : FluentValidatorBase<NextRoundCommand>
{
    public NextRoundCommandValidator()
    {
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(40);
    }
}
