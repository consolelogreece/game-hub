using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Battleships
{
    public class BattleshipsMoveResult : ActionResult
    {
        public BattleshipsMoveResult(bool wasSuccessful, string message = "", bool didEndGame = false) : base(wasSuccessful, message)
        {
            DidEndGame = didEndGame;
        }

        public bool DidEndGame;
    }
}