using System.Collections.Generic;
using ChessDotNet;
using GameHub.Games.BoardGames.Battleships;
using GameHub.Games.BoardGames.Chess;
using GameHub.Games.BoardGames.Common;
public class BattleshipsService
{
    private GameStarter _gameStarter;
    private GameJoiner _gameJoiner;
    private GameResigner _gameResigner;
    private GameRestarter _gameRestarter;
    private GamePlayerGetter<BattleshipsPlayerModel> _gamePlayerGetter;
    private GameStateGetter<BattleshipsGameState> _gameStateGetter;
    private GameMover<BattleshipsPosition> _gameMover;
    // id of player using the service
    private string _playerId;

    private Battleships _game;

    public BattleshipsService(Battleships game, string playerId)
    {
        _gameStarter = new GameStarter(game);

        _gameJoiner = new GameJoiner(game);

        _gameResigner = new GameResigner(game);

        _gameRestarter = new GameRestarter(game);

        _gamePlayerGetter = new GamePlayerGetter<BattleshipsPlayerModel>(game);

        _gameMover = new GameMover<BattleshipsPosition>(game);

        _playerId = playerId;

        _game = game;
    }

    public ActionResult StartGame() => _gameStarter.StartGame(_playerId);

    public ActionResult JoinGame(string playerNick) => _gameJoiner.JoinGame(_playerId, playerNick);

    public ActionResult RegisterShips(List<ShipModel> shipModels)
    {
        lock(_game)
        {
            return _game.RegisterShips(shipModels, _playerId);
        }
    }

    public BattleshipsPlayerModel GetPlayer() => (BattleshipsPlayerModel)_gamePlayerGetter.Get(_playerId);
    
    public ActionResult Resign() => _gameResigner.Resign(_playerId);

    public ActionResult Restart() => _gameRestarter.Restart(_playerId);

    public BattleshipsGameState GetGameState() 
    {
        lock(_game)
        {
            return _game.GetGameState(_playerId);
        }
    }

    public ActionResult Move(BattleshipsPosition move) => _gameMover.Move(_playerId, move);
}