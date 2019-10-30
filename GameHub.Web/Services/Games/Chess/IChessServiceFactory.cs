namespace GameHub.Web.Services.Games.ConnectFourServices
{
    public interface IChessServiceFactory 
    {
        ChessService Create(string gameId, string playerId);
    }
}