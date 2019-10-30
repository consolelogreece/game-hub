namespace GameHub.Web.Services.Games.ConnectFourServices
{
    public interface IConnectFourServiceFactory 
    {
        ConnectFourService Create(string gameId, string playerId);
    }
}