using Schnauz.Shared.Interfaces;
using FluentValidation;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Shared.Commands;
public class SearchMatchCommand : ICommand
{
    public string Username { get; set; } = string.Empty;
    
    public RegionDto Region { get; set; }
}

public class SearchGameCommandValidator : FluentValidatorBase<SearchMatchCommand>
{
    public SearchGameCommandValidator()
    {
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(40);
        RuleFor(x => x.Region).IsInEnum();
    }
}
