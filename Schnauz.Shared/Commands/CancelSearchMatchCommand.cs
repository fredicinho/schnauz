using Schnauz.Shared.Interfaces;
using FluentValidation;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Shared.Commands;
public class CancelSearchMatchCommand : ICommand
{
    public string Username { get; set; } = string.Empty;
}

public class CancelSearchGameCommandValidator : FluentValidatorBase<CancelSearchMatchCommand>
{
    public CancelSearchGameCommandValidator()
    {
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(40);
    }
}
