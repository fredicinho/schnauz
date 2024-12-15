namespace Schnauz.GrainInterfaces;

public interface IMatchMaker: IGrainWithStringKey
{
    [Transaction(TransactionOption.Join)]
    ValueTask SearchMatch(string userName);
    
    [Transaction(TransactionOption.Join)]
    ValueTask CancelSearch(string userName);
    
    [Transaction(TransactionOption.Create)]
    ValueTask MatchPlayers();
}