namespace GameHub.Web.Services.Games.ConnectFourServices
{
    public interface IBattleshipsServiceFactory 
    {
        BattleshipsService Create(string gameId, string playerId);
    }
}