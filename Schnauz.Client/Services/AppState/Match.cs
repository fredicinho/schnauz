using Schnauz.Shared.Dtos;

namespace Schnauz.Client.Services.AppState;

public class Match
{
    private MatchDto? MatchDto { get; set; }
    
    public event Action? OnChange;
    
    private void NotifyStateChanged() => OnChange?.Invoke();
    
    public void SetMatchDto(MatchDto? matchDto)
    {
        MatchDto = matchDto;
        NotifyStateChanged();
    }
    
    public MatchDto? GetMatchDto() => MatchDto;
}