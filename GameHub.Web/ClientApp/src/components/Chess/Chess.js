﻿import React, { Component } from "react";
import Chessboard from "chessboardjsx";
import OptionPanel from '../Common/OptionPanel';
import Popup from '../Popup';
import PromotionSelection from './PromotionSelection';
import { Title } from '../Common/Text';

/*
    todos:
        offer draw -PRIORITY
        try to reduce the amount of hub calls made.
*/

export default class Chess extends Component {
    constructor(props) {
        super(props);
        this.state = {
            hubConnection: null,
            fen: "empty",
            legalMoves: {},
            squareStyles: {},
            pieceSquare:"",
            isGameFull: false,
            gameState: "lobby",
            playerInfo: null,
            gameMessage: "",
            playerTurn: "",
            displayPromotionPrompt: false,
            promotionMove: null
        }
    }

    async componentDidMount() 
    {
        this.props.on('PlayerMoved', this.playerMoved);

        this.props.on('GameStarted', this.gameStarted);

        this.props.on('IllegalAction', this.illegalAction);

        this.props.on('GameOver', this.gameOverHandler);

        this.props.on(['PlayerJoined', 'PlayerResigned'], gameState => this.updateStateWithNewGameState(gameState))

        this.props.on('RematchStarted', gameState => this.RematchStarted(gameState));
   
        this.props.startConnection().then(() => this.initilaize()).then(() => this.props.onLoadComplete());
    }

    initilaize()
    {
        return this.populatePlayerClientInfo()
        .then(() => this.populateGameState())
        .then(() => this.populateAvailableMoves());
    }

    playerMoved = res =>
    {
        this.updateStateWithNewGameState(res);

        this.populateAvailableMoves();
    }

    getTurnIndicator = player => 
    {
        if (this.state.playerInfo === null)
        {
            if (player === null)
            {
                return "";
            }
            
            return player.playerNick;
        }
        return player.playerNick === this.state.playerInfo.playerNick ? "your" : player.playerNick;
    }
    
    gameStarted = gameState => 
    {
        this.updateStateWithNewGameState(gameState);

        this.populateAvailableMoves();
    }

    RematchStarted = gameState =>
    {
        if (this.state.playerInfo !== null)
        {
            this.setState({playerInfo:{...this.state.playerInfo, resigned: false}});
        }

        this.populateAvailableMoves();

        this.updateStateWithNewGameState(gameState);
    }

    gameOverHandler = gameState =>
    {
        this.updateStateWithNewGameState(gameState);
    }

    populateGameState = () =>
    {
        return this.invoke('GetGameState')
            .then(res => this.updateStateWithNewGameState(res));
    }

    updateStateWithNewGameState = gameState =>
    {
        var message = this.generateGameMessageFromGameState(gameState);
        var isGameFull = this.isGameFull(gameState);
        this.setState({
            fen: gameState.boardStateFen, 
            gameState: gameState.status.status,
            playerTurn: this.getTurnIndicator(gameState.currentTurnPlayer),
            gameMessage: message,
            isGameFull: isGameFull
        })
    }

    generateGameMessageFromGameState = gameState =>
    {
        let message = "";

        let {status, endReason} = gameState.status;

        let isGameFull = this.isGameFull(gameState);

        if (status === "finished") message = endReason;

        if (status === "lobby")
        {
            if (this.state.playerInfo === null && !this.isHost())
            {
                if (isGameFull)
                {
                    message = "Game full, waiting for host to start..."
                }
            }
            else if (!this.isHost())
            {
                message = "Waiting for host to start...";
            } 
        }

        if (status === "started")
        {
            let playerTurn = this.getTurnIndicator(gameState.currentTurnPlayer);
            
            message = `It's ${playerTurn} turn`;
        }

        return message;
    }

    populateAvailableMoves = () =>
    {
        return this.invoke('GetMoves').then(res => this.mapMoves(res))
    }

    populatePlayerClientInfo = () =>
    {
        return this.invoke('GetClientPlayerInfo')
        .then(res => {
            this.setState({
                playerInfo: res
            })
        });
    }

    mapMoves = moves =>
    {
        if (moves === null || this.state.gameState !== "started") return;

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
        if (this.state.pieceSquare !== "") return; 

        var moves = this.getLegalMoves(square);

        var styles = this.highlightLegalMoves(moves);

        this.setState({squareStyles: styles});
    }

    onMouseOutSquare = square => 
    {
        if (this.state.pieceSquare === "")
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
        if (this.state.pieceSquare !== "")
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

        var move = legalMoves.find(el => this.convertPositionToSquare(el.newPosition) === to);

        if (move)
        {
            if (move.promotion !== null)
            {
                this.setState({displayPromotionPrompt: true, promotionMove: move});

                return;
            }

            this.invoke('Move', move);
        }

        this.setState({pieceSquare: "", squareStyles: {}})
    }

    invoke = (destination, ...rest) =>
    {
        return this.props.invoke(destination, ...rest)
        .then(res => {
            if (res !== null && !res.wasSuccessful)
            {
                this.props.displayTimedError(res.message);
            }

            return res;
        })
        .catch(res => this.props.displayTimedError(res));
    }

    makePromotion = promotion =>
    {
        if (promotion !==  'R' && promotion !== 'B' && promotion !== 'Q' && promotion !== 'N')
        {
            return;
        }

        if (this.state.promotionMove === null)
        {
            this.setState({displayPromotionPrompt: false});

            return;
        }

        var move = this.state.promotionMove;

        move.promotion = promotion;

        this.invoke('Move', move)
        .then(res => {
            this.setState({displayPromotionPrompt: false, promotionMove: null})
        });
    }

    cancelPromotion = () =>
    {
        this.setState({displayPromotionPrompt: false, promotion: null})
    }

    JoinGame = () => {
        return this.invoke('JoinGame');
    }

    GameJoined = () =>
    {
        this.populatePlayerClientInfo()
        .then(this.populateGameState())
    }

    StartGame = () =>
    {
        this.invoke('StartGame');
    }

    Rematch = () =>
    {
        this.invoke('Rematch');
    }

    Resign = () =>
    {
        if (this.state.playerInfo !== null)
        {
            this.setState({playerInfo:{...this.state.playerInfo, resigned: false}});

            this.invoke('Resign');
        }
    }

    isGameFull = gameState =>
    {
        return gameState.configuration.nPlayersMax <= gameState.players.length;
    }

    isHost = () =>
    {
        return this.state.playerInfo !== null && this.state.playerInfo.isHost;
    }

    render()
    {
        let isHost = this.isHost();

        let orientation = this.state.playerInfo !== null && this.state.playerInfo.player === 0 ? "black" : "white";

        let width = this.props.containerWidth <= 600 ? this.props.containerWidth : 600;

        let gameState = this.state.gameState;

        let isPlayerRegistered = !!this.state.playerInfo;

        let isGameFull = this.state.isGameFull;

        let hasPlayerResigned = this.state.playerInfo !== null && this.state.playerInfo.resigned;

        return (
            <div style={{width: width, margin: "0 auto"}}>
                {this.props.errorMessage}
                <Title text="CHESS" />
                {this.state.displayPromotionPrompt &&
                    <Popup 
                        superContainerStyles={{backgroundColor: "rgba(0,0,0, 0.5)"}} 
                        popupStyles={{width:" 100%", maxWidth: "800px"}}
                        onClose={this.cancelPromotion}>
                        <div style={{backgroundColor:"white", borderRadius: "10px", padding: "10px"}} >
                        <span id="chess-promotion-selection-popup">Promotion</span>
                            <PromotionSelection callback={this.makePromotion} />
                        </div>
                    </Popup>
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
                    isHost = {isHost}
                    JoinGame = {this.JoinGame}
                    GameJoined = {this.GameJoined}
                    gameState = {gameState}
                    isPlayerRegistered = {isPlayerRegistered}
                    StartGame = {this.StartGame}
                    isGameFull = {isGameFull}
                    hasPlayerResigned = {hasPlayerResigned}
                    Rematch = {() => this.invoke('Rematch')}
                    Resign = {this.Resign}
                />
            </div>
        )
    }
}