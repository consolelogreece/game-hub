import React, { Component } from "react";
import Chessboard from "chessboardjsx";
import { HubConnectionBuilder } from '@aspnet/signalr';
import JoinGame from '../Common/JoinGame'

export default class Chess extends Component {
    constructor(props) {
        super(props);
        this.state = {
            hubConnection: null,
            fen: "empty",
            gameId: props.match.params.gameId,
            legalMoves: {},
            squareStyles: {},
            pieceSquare:"",
            gameState: "lobby",
            playerInfo: null,
            gameMessage: "",
            playerTurn: ""
        }
    }

    componentDidMount() {
        let gameState = [];

        const hubConnection = new HubConnectionBuilder()
        .withUrl("/chesshub", {accessTokenFactory: () => "testing"})
        .build();

        hubConnection.on('PlayerMoved', this.playerMoved);

        hubConnection.on('GameStarted', this.gameStarted);

        hubConnection.on('IllegalAction', this.illegalAction);

        this.setState({ hubConnection }, () => {
            this.state.hubConnection
            .start()
            .then(() => {
                this.state.hubConnection.invoke('JoinRoom', this.state.gameId)
                .then(this.initilaize())
            })
        });
    }

    initilaize()
    {
        return this.populateGameState()
        .then(this.populatePlayerClientInfo())
        .then(this.populateAvailableMoves());
    }

    playerMoved = res =>
    {
        var playerTurn = res.nextTurnPlayer.id == this.state.playerId ? "your" : res.nextTurnPlayer.playerNick + "'s";

        this.setState({
            fen: res.fen,
            gameMessage: res.message,
            playerTurn: playerTurn
        });

        this.populateAvailableMoves();
    }

    gameStarted = res => 
    {
        this.setState({
            gameState: "started"
        });
    }

    illegalAction = message => 
    {
        this.setState({
           gameMessage: message 
        });
    }

    populateGameState()
    {
        var gameId = this.state.gameId;

        return this.state.hubConnection.invoke('GetGameState', gameId)
            .then(res => {
                this.setState({
                    fen: res.boardStateFen, 
                    gameState: res.status
                })
            });
    }

    populateAvailableMoves()
    {
        var gameId = this.state.gameId;

        this.state.hubConnection.invoke('GetMoves', this.state.gameId).then(res => this.mapMoves(res))
    }

    populatePlayerClientInfo()
    {
        var gameId = this.state.gameId;

        return this.state.hubConnection.invoke('GetClientPlayerInfo', gameId)
        .then(res => {
            this.setState({
                playerInfo: res
            })
        });
    }

    mapMoves(moves)
    {
        if (moves == null || this.state.gameState != "started") return;

        let map = {};

        moves.forEach(el => {
            let key = this.convertPositionToSquare(el.originalPosition);

            if (!(key in map))
            {
                map[key] = [];
            }

            map[key].push(el);
        });

        this.setState({legalMoves: map})
    }

    convertPositionToSquare(pos)
    {
        let files = "abcdefgh"

        return `${files[pos.file]}${pos.rank}`;
    }

    onMouseOverSquare = square =>
    {
        if (this.state.pieceSquare != "") return; 

        var moves = this.getLegalMoves(square);

        var styles = this.highlightLegalMoves(moves);

        this.setState({squareStyles: styles});
    }

    onMouseOutSquare = square => 
    {
        if (this.state.pieceSquare == "")
        {
            this.setState({squareStyles: {}});
        }
    }

    onSquareRightClick = square =>
    {
        this.setState({squareStyles: {}, pieceSquare: ""});
    }

    getLegalMoves(square)
    {
        let moves = this.state.legalMoves[square];

        if (moves === undefined) return [];

        return moves;
    }

    highlightLegalMoves(moves)
    {
        let styles = {};
        moves.forEach(el => {
            var sq = this.convertPositionToSquare(el.newPosition);
            styles[sq] = {
                    background: "radial-gradient(circle, #fffc00 36%, transparent 40%)",
                    borderRadius: "50%"
            };
        });

        return styles;
    }

    onSquareClick = square => 
    {
        var moves = this.getLegalMoves(square);

        // a square has already been clicked, meaning this is must be a move
        if (this.state.pieceSquare != "")
        {
            this.makeMove(this.state.pieceSquare, square);
            return;
        }

        var styles = this.highlightLegalMoves(moves);

        styles[square] = {
            backgroundColor:"pink"
        }

        this.setState({squareStyles: styles, pieceSquare: square}); 
    }

    makeMove = (from, to) =>
    {
        var legalMoves = this.getLegalMoves(from);

        var move = legalMoves.find(el => this.convertPositionToSquare(el.newPosition) == to);

        if (move)
        {
            this.state.hubConnection.invoke('MakeMove', move, this.state.gameId)
            .then(this.state.hubConnection.invoke('GetMoves', this.state.gameId)
            .then(res => this.mapMoves(res)));
        }

        this.setState({pieceSquare: "", squareStyles: {}})
    }

    JoinGame(name) {
        console.log("eet")
        this.state.hubConnection.invoke('JoinGame', this.state.gameId, name)
            .then(res =>  
                this.setState({gameState: "joined"})
            )
            .catch(() => this.setState({gameMessage: "oopsie daisy"}));
    }

    StartGame()
    {
        this.state.hubConnection.invoke('StartGame', this.state.gameId).catch(res => this.setState({gameMessage:res}));
    }

    render()
    {
        let optionsPanel;

        let isHost = this.state.playerInfo != null && this.state.playerInfo.isHost;

        switch (this.state.gameState)
        {
            case "lobby":
                optionsPanel = (
                    <div>                        
                        {!this.state.playerInfo &&
                            <JoinGame 
                                title="What's your name?"
                                JoinGame={(name) => this.JoinGame(name)}
                            />
                        }
                        {isHost &&
                            <button onClick={() => this.StartGame()}>Start Game</button>    
                        }
                       
                    </div>
                )
                break;

            case "started":
                optionsPanel = (
                    <div>
                        <h6>its {this.state.playerTurn}'s turn</h6>   
                    </div>
                )
                break;

            case "finished":
                optionsPanel = (
                    <div>
                        <h6>Game over!</h6>
                        <button onClick={() => { }}>Re-match</button>
                    </div>
                )
                break;

            default:
                optionsPanel = (
                <div>
                </div>
            )
        }

        return (
            <div>
                <h3>CHESS</h3>
                {(this.state.gameState == "started") && <div>it's {this.state.playerTurn} turn</div>}
                <Chessboard 
                    onMouseOverSquare = {this.onMouseOverSquare}
                    onMouseOutSquare = {this.onMouseOutSquare}
                    onSquareClick = {this.onSquareClick}
                    onSquareRightClick = {this.onSquareRightClick}
                    squareStyles = {this.state.squareStyles}
                    position = {this.state.fen}
                />
                {optionsPanel}
                <button onClick={() => console.log(this.state)}>log state</button>
            </div>
            )
    }
}