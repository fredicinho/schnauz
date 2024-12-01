using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Shared.Dtos;

public class ProfileDto
{
    public string UserName { get; set; } = String.Empty;
    
    public UserStateDto UserState { get; set; }
    
    public MatchDto? CurrentMatch { get; set; }
}