import React, { Component } from "react";
import Chessboard from "chessboardjsx";
import { HubConnectionBuilder } from '@aspnet/signalr';
import OptionPanel from '../Common/OptionPanel';
import Popup from '../Common/Popup'
import PromotionSelection from './PromotionSelection';
import { Title, Subtitle } from '../Common/Text';

/*
    todos:
        win detection -PRIORITY
        stalement detection -PRIORITY   ========= use conclusionstatus to display whether someone has one or whatever. it used to work unless i refreshed. 
        offer draw -PRIORITY
        fix bug, for some reason on initial start, it doesnt indicate whos turn it is properly
        try to reduce the amount of hub calls made.

*/

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
            playerTurn: "",
            displayPromotionPrompt: false,
            promotionMove: null
        }
    }

    componentDidMount() 
    {
        const hubConnection = new HubConnectionBuilder()
        .withUrl("/chesshub", {accessTokenFactory: () => "testing"})
        .build();

        hubConnection.on('PlayerMoved', this.playerMoved);

        hubConnection.on('GameStarted', this.gameStarted);

        hubConnection.on('IllegalAction', this.illegalAction);

        hubConnection.on('GameOver', this.gameOverHandler);

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
        return this.populatePlayerClientInfo()
        .then(this.populateGameState())
        .then(this.populateAvailableMoves());
    }

    playerMoved = res =>
    {
        this.updateStateWithNewGameState(res);

        this.populateAvailableMoves();
    }

    getTurnIndicator = player => 
    {
        if (this.state.playerInfo === null) return "";

        return player.id == this.state.playerInfo.id ? "your" : (player.playerNick + "'s");
    }

    gameStarted = gameState => 
    {
        this.populateAvailableMoves();

        this.updateStateWithNewGameState(gameState);
    }

    illegalAction = message => 
    {
        this.setState({
           gameMessage: message 
        });
    }

    gameOverHandler = details =>
    {
        var type = details.type;
    }

    populateGameState = () =>
    {
        var gameId = this.state.gameId;

        return this.state.hubConnection.invoke('GetGameState', gameId)
            .then(res => this.updateStateWithNewGameState(res));
    }

    updateStateWithNewGameState = gameState =>
    {
        var message = this.generateGameMessageFromGameState(gameState);
        this.setState({
            fen: gameState.boardStateFen, 
            gameState: gameState.status.status,
            playerTurn: this.getTurnIndicator(gameState.currentTurnPlayer),
            gameMessage: message
        })
    }

    generateGameMessageFromGameState = gameState =>
    {
        let message = "";

        let {status, winner} = gameState.status;

        if ( winner != undefined) message =  `${gameState.status.winner.playerNick} has won!`;
    
        // if the game has finished and there is no winner, must be stalemate.
        if (status == "finished" && winner == undefined) message = "Stalemate!"

        if (status == "lobby")
        {
            if (this.state.playerInfo == null)
            {
                message = "Please enter your name"
            }
            else if (!this.isHost())
            {
                message = "Waiting for host to start...";
            } 
        }

        if (status == "started")
        {
            let playerTurn = this.getTurnIndicator(gameState.currentTurnPlayer);
            
            message = `It's ${playerTurn} turn`;
        }

        return message;
    }

    populateAvailableMoves = () =>
    {
        var gameId = this.state.gameId;

        this.state.hubConnection.invoke('GetMoves', gameId).then(res => this.mapMoves(res))
    }

    populatePlayerClientInfo = () =>
    {
        var gameId = this.state.gameId;

        return this.state.hubConnection.invoke('GetClientPlayerInfo', gameId)
        .then(res => {
            this.setState({
                playerInfo: res
            })
        });
    }

    mapMoves = moves =>
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

    convertPositionToSquare = pos =>
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

    getLegalMoves = square =>
    {
        let moves = this.state.legalMoves[square];

        if (moves === undefined) return [];

        return moves;
    }

    highlightLegalMoves = moves =>
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
            if (move.promotion != null)
            {
                this.setState({displayPromotionPrompt: true, promotionMove: move});

                return;
            }

            this.state.hubConnection.invoke('MakeMove', move, this.state.gameId);
        }

        this.setState({pieceSquare: "", squareStyles: {}})
    }

    makePromotion = promotion =>
    {
        if (promotion !=  'R' && promotion != 'B' && promotion != 'Q' && promotion != 'N')
        {
            return;
        }

        if (this.state.promotionMove == null)
        {
            this.setState({displayPromotionPrompt: false});

            return;
        }

        var move = this.state.promotionMove;

        move.promotion = promotion;

        this.state.hubConnection.invoke('MakeMove', move, this.state.gameId)
        .then(res => {
            this.setState({displayPromotionPrompt: false, promotionMove: null})
        });
    }

    JoinGame = name => {
        this.state.hubConnection.invoke('JoinGame', this.state.gameId, name)
            .then(this.populatePlayerClientInfo())
            .then(this.populateGameState())
            .catch(() => this.setState({gameMessage: "oopsie daisy"}));
    }

    StartGame = () =>
    {
        this.state.hubConnection.invoke('StartGame', this.state.gameId).catch(res => this.setState({gameMessage:res}));
    }

    isHost = () =>
    {
        return this.state.playerInfo != null && this.state.playerInfo.isHost;
    }

    render()
    {
        let isHost = this.isHost();

        let orientation = this.state.playerInfo != null && this.state.playerInfo.player == 0 ? "black" : "white";

        let width = this.props.containerWidth <= 600 ? this.props.containerWidth : 600;

        let gameState = this.state.gameState;

        let nextPlayerName = this.state.playerTurn;

        let clientName = this.state.playerInfo != null ? this.state.playerInfo.playerNick : "";

        let isPlayerRegistered = !!this.state.playerInfo;

        return (
            <div style={{width: width, margin: "0 auto"}}>
                <Title text="CHESS" />
                <Subtitle text={clientName} />
                {this.state.displayPromotionPrompt &&
                    <Popup title="promotion" content={
                        <PromotionSelection callback={this.makePromotion} />
                    } />
                }
                <div style={{margin: "0 auto", Width: "100%", backgroundColor: "red"}}>
                    <Chessboard 
                        width={width}
                        draggable={false}
                        orientation={orientation}
                        onMouseOverSquare = {this.onMouseOverSquare}
                        onMouseOutSquare = {this.onMouseOutSquare}
                        onSquareClick = {this.onSquareClick}
                        onSquareRightClick = {this.onSquareRightClick}
                        squareStyles = {this.state.squareStyles}
                        position = {this.state.fen}
                    />
                </div>
                {this.state.gameMessage}
                <OptionPanel
                    playerName = {nextPlayerName}
                    isHost = {isHost}
                    JoinGame = {this.JoinGame}
                    gameState = {gameState}
                    isPlayerRegistered = {isPlayerRegistered}
                    StartGame = {this.StartGame}
                />
            </div>
            )
    }
}