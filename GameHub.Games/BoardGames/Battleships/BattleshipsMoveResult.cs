using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Battleships
{
    public class BattleshipsMoveResult : ActionResult
    {
        public BattleshipsMoveResult(bool wasSuccessful, string message = "", Ship hitShip = null, bool didEndGame = false) : base(wasSuccessful, message)
        {
            HitShip = hitShip;
            DidEndGame = didEndGame;
        }
        public Ship HitShip;

        public bool DidEndGame;
    }
}