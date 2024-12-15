using Orleans;

namespace Schnauz.Shared.Dtos.enums;

[GenerateSerializer]
public enum RegionDto
{
    [Id(0)]
    EUROPE,
    [Id(1)]
    NORTH_AMERICA,
    [Id(2)]
    SOUTH_AMERICA,
    [Id(3)]
    ASIA,
}