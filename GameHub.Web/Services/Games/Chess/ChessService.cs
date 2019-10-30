using System.Collections.Generic;
using ChessDotNet;
using GameHub.Games.BoardGames.Chess;
using GameHub.Games.BoardGames.Common;
public class ChessService
{
    private GameStarter _gameStarter;
    private GameJoiner _gameJoiner;
    private GameResigner _gameResigner;
    private GameRestarter _gameRestarter;
    private GamePlayerGetter<ChessPlayer> _gamePlayerGetter;
    private GameStateGetter<GameStateChess> _gameStateGetter;
    private GameMover<Move> _gameMover;
    // id of player using the service
    private string _playerId;

    private Chess _game;

    public ChessService(Chess game, string playerId)
    {
        _gameStarter = new GameStarter(game);

        _gameJoiner = new GameJoiner(game);

        _gameResigner = new GameResigner(game);

        _gameRestarter = new GameRestarter(game);

        _gamePlayerGetter = new GamePlayerGetter<ChessPlayer>(game);

        _gameStateGetter = new GameStateGetter<GameStateChess>(game);

        _gameMover = new GameMover<Move>(game);

        _playerId = playerId;

        _game = game;
    }

    public ActionResult StartGame() => _gameStarter.StartGame(_playerId);

    public ActionResult JoinGame(string playerNick) => _gameJoiner.JoinGame(_playerId, playerNick);

    public ChessPlayer GetPlayer() => (ChessPlayer)_gamePlayerGetter.Get(_playerId);
    public ActionResult Resign() => _gameResigner.Resign(_playerId);

    public ActionResult Restart() => _gameRestarter.Restart(_playerId);

    public GameStateChess GetGameState() => (GameStateChess)_gameStateGetter.Get();

    public ActionResult Move(Move move) => _gameMover.Move(_playerId, move);

    public List<Move> GetMoves()
    {        
        var player = _game.GetPlayer(_playerId);

        var moves = new List<Move>();

        if (player != null)
        {
            moves = _game.GetMoves(player);
        }

        return moves;
    }
}