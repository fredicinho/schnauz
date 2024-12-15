using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Grains.MatchMaker;

public static class MatchMakerKey
{
    public static string GetKey(RegionDto region) => $"matchmaker-{region}";
}