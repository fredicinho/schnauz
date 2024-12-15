using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Grains.Player
{
    [Serializable]
    public class PlayerState
    {
        public UserStateDto UserState { get; set; } = UserStateDto.SIGNED_IN;
        
        public RegionDto SelectedRegion { get; set; } = RegionDto.EUROPE;
        
        public int NumberOfLifes { get; set; } = 3;
    
        public List<CardDto> CardsOnHand { get; set; } = new();
    
        public bool IsOut { get; set; } = false;

        public PlayerActionDto LastAction { get; set; } = PlayerActionDto.NO_ACTION;
        
        public Guid? CurrentMatchId { get; set; }
    }
}
