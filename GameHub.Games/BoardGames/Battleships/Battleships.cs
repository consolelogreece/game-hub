using System.Collections.Generic;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Battleships
{
    public class Battleships : IJoinable, 
        IRestartable, 
        IStartable, 
        IResignable, 
        IMoveable<BattleshipsMove>,
        IGamePlayerGetter<BattleshipsPlayer>, 
        IGameStateGetter<BattleshipsGameState>
    {
        #region private props
        private Battleships _game;

        private BattleshipsConfiguration _config;

        private List<BattleshipsPlayer> _players;
        #endregion

        public Battleships(BattleshipsConfiguration config)
        {
            _config = config;

            _game = new Battleships(config);

            _players = new List<BattleshipsPlayer>();
            
        }

        public BattleshipsGameState GetGameState()
        {
            throw new System.NotImplementedException();
        }

        public BattleshipsPlayer GetPlayer(string playerId)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Join(string playerId, string playerNick)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Move(string playerId, BattleshipsMove move)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Resign(string playerId)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Restart(string playerId)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Start(string playerId)
        {
            throw new System.NotImplementedException();
        }

        BattleshipsGameState IGameStateGetter<BattleshipsGameState>.GetGameState()
        {
            throw new System.NotImplementedException();
        }
    }
}