using Orleans;

namespace Schnauz.Shared.Dtos.enums;

[GenerateSerializer]
public enum SuitDto
{
    [Id(0)]
    HEARTS,
    [Id(1)]
    DIAMONDS,
    [Id(2)]
    CLUBS,
    [Id(3)]
    SPADES,
}