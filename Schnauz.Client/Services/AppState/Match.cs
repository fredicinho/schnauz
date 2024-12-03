using Schnauz.Shared.Dtos;

namespace Schnauz.Client.Services.AppState;

public class Match
{
    public MatchDto? MatchDto { get; private set; }
    
    public event Action? OnChange;
    
    private void NotifyStateChanged() => OnChange?.Invoke();
    
    public void SetMatchDto(MatchDto? matchDto)
    {
        MatchDto = matchDto;
        NotifyStateChanged();
    }
    
}