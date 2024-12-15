using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Grains.CardDealer;

public static class CardHelper
{
    private static readonly Random Random = new Random();

    public static List<CardDto> GenerateDeck()
    {
        return Enum.GetValues<SuitDto>()
            .SelectMany(suit => Enum.GetValues<CardRankDto>()
                .Select(rank => new CardDto { Suit = suit, CardRank = rank }))
            .ToList();
    }
    
    /**
     * Fisher-Yates shuffle
     */
    public static List<CardDto> ShuffleDeck(List<CardDto> deck)
    {
        var n = deck.Count;
        for (var i = n - 1; i > 0; i--)
        {
            var j = Random.Next(i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }
    
    public static (List<CardDto> deck, List<CardDto> cards) DealCards(List<CardDto> deck, int numberOfCards = 3)
    {
        return (deck.Skip(numberOfCards).ToList(), deck.Take(numberOfCards).ToList());
    }
    
    /**
     * Calculate the points of a list of cards according to the Schnauz rules.
     * Only points with the same suit are added.
     * If all three cards are different suits, the highest card is added.
     * If three cards are the same rank (but different suits), the points are 30.5.
     * 10, Jack, Queen, King are worth 10 points.
     * Ace is worth 11 points.
     */
    public static double CalculateCardPoints(List<CardDto> cards)
    {
        if (cards.Count != 3)
        {
            throw new ArgumentException("Three cards are required to calculate the points");
        }
        
        // If all three cards are Aces, the points are 33.
        if (cards.All(card => card.CardRank == CardRankDto.Ace))
        {
            return 33;
        }
        
        // If all three cards are the same rank, the points are 30.5.
        if (cards[0].CardRank == cards[1].CardRank && cards[1].CardRank == cards[2].CardRank)
        {
            return 30.5;
        }
        
        // If all three cards are the same suit, the sum of the points is returned.
        if (cards[0].Suit == cards[1].Suit && cards[1].Suit == cards[2].Suit)
        {
            return cards.Sum(card => GetPointsForRank(card.CardRank));
        }
        
        // If two cards are the same suit, the sum of the points of the two equal cards is returned.
        if (cards[0].Suit == cards[1].Suit)
        {
            return GetPointsForRank(cards[0].CardRank) + GetPointsForRank(cards[1].CardRank);
        }

        if (cards[1].Suit == cards[2].Suit)
        {
            return GetPointsForRank(cards[1].CardRank) + GetPointsForRank(cards[2].CardRank);
        }

        if (cards[0].Suit == cards[2].Suit)
        {
            return GetPointsForRank(cards[0].CardRank) + GetPointsForRank(cards[2].CardRank);
        }
        
        // All three cards are different suits. Therefore, the highest card is counted.
        return cards.Max(card => GetPointsForRank(card.CardRank));
    }
    
    private static double GetPointsForRank(CardRankDto rank)
    {
        return rank switch
        {
            CardRankDto.Seven => 7,
            CardRankDto.Eight => 8,
            CardRankDto.Nine => 9,
            CardRankDto.Ten => 10,
            CardRankDto.Jack => 10,
            CardRankDto.Queen => 10,
            CardRankDto.King => 10,
            CardRankDto.Ace => 11,
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null)
        };
    }
}