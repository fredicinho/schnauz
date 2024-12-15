namespace Schnauz.Grains.MatchMaker
{
    [Serializable]
    public class MatchMakerState
    {
        public Queue<string> PlayersSearchingGame { get; set; } = new();
    }
}
