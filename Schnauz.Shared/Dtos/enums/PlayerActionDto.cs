namespace Schnauz.Shared.Dtos.enums;

public enum PlayerActionDto
{
    CHANGED_CARD, // Changed a card
    CHANGED_ALL_CARDS, // Changed all cards
    SHOVE, // No card change but not closing the game
    CLOSING, // Closing the game and is therefore last round
    SCHNAUZ, // Closed the game with 31 points
    FIRE // Closed the game while all other players lose a life point
}