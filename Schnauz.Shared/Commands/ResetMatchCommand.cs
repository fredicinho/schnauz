using Schnauz.Shared.Interfaces;
using FluentValidation;

namespace Schnauz.Shared.Commands;
public class ResetMatchCommand : ICommand
{
    public string Username { get; set; } = string.Empty;
}

public class ResetMatchCommandValidator : FluentValidatorBase<ResetMatchCommand>
{
    public ResetMatchCommandValidator()
    {
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(40);
    }
}
