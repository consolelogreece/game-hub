﻿import React, { Component } from 'react';
import './ConnectFour.css';
import { Title, Subtitle } from '../Common/Text';
import ResizeWithContainerHOC from '../HigherOrder/GetRenderedWidthHOC';
import OptionPanel from '../Common/OptionPanel';

import Board from './Board';

export default class ConnectFour extends Component {
    constructor(props) {
        super(props);

        this.state = {
            column: 0,
            isGameFull: false,
            gameMessage: "",
            playerTurn: "",
            gameState: "lobby",
            playerInfo: null,
            boardState: [[]],
            boardColor: "#0e3363"
        };
    }

    componentDidMount() 
    {
        this.props.on('IllegalAction', res => {this.setState({ gameMessage: res })});

        this.props.on([
            'PlayerResigned',
            'PlayerJoined',
            'GameStarted',
            'GameOver',
            'PlayerMoved'
        ], gameState => this.updateStateWithNewGameState(gameState));

        this.props.on('RematchStarted', gameState => this.RematchStarted(gameState));
        
        this.props.startConnection()
        .then(() => this.populatePlayerClientInfo())
        .then(res =>  this.populateGameState());
    }

    move = col =>
    {
        // if the playerinfo is null, the player is a spectator and thus can't move.
        if (this.state.playerInfo == null) return;

        this.props.invoke('Move', col).catch(err => console.error(err));;
    }
    
    JoinGame = name => {
        this.props.invoke('JoinGame', name)
            .then(this.populatePlayerClientInfo())
            .then(this.populateGameState())
            .catch(res => this.setState({gameMessage: "oopsie daisy"}));
    }

    RematchStarted = gameState =>
    {
        if (this.state.playerInfo !== null)
        {
            this.setState({playerInfo:{...this.state.playerInfo, resigned: false}});
        }

        this.updateStateWithNewGameState(gameState);
    }

    invoke = destination =>
    {
        return this.props.invoke(destination).catch(res => this.setState({gameMessage:res}));
    }

    populateGameState = () =>
    {
        return this.props.invoke('GetGameState')
        .then(gameState => this.updateStateWithNewGameState(gameState));
    }

    updateStateWithNewGameState = gameState =>
    {
        var message = this.generateGameMessageFromGameState(gameState);
        var isGameFull = this.isGameFull(gameState);
        this.setState({
            boardState: gameState.boardState,
            gameState: gameState.status.status,
            playerTurn: this.getTurnIndicator(gameState.currentTurnPlayer),
            isGameFull: isGameFull,
            gameMessage: message
        })
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

    generateGameMessageFromGameState = gameState =>
    {
        let message = "";

        let {status, endReason} = gameState.status;

        let isGameFull = this.isGameFull(gameState);

        if (status === "finished") message = endReason;

        if (status === "lobby")
        {
            if (this.state.playerInfo === null)
            {
                if (isGameFull)
                {
                    message = "Game full, waiting for host to start..."
                }
                else
                {
                    message = "Please enter your name"
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

    populatePlayerClientInfo = () =>
    {
        return this.props.invoke('GetClientPlayerInfo')
            .then(res => 
                {
                    if (res === null) return; 
                    this.setState ({
                        playerInfo: res
                    })
                })
    }

    isHost = () =>
    {
        return this.state.playerInfo != null && this.state.playerInfo.isHost;
    }

    Resign = () => 
    {
        if (this.state.playerInfo !== null)
        {
            this.setState({playerInfo:{...this.state.playerInfo, resigned: true}});

            this.invoke("Resign");
        }
    }

    isGameFull = gameState =>
    {
        return gameState.configuration.nPlayersMax <= gameState.players.length;
    }

    render()
    {
        var Aboard = ResizeWithContainerHOC(Board);

        let gameState = this.state.gameState;

        let isHost = this.isHost();

        let isGameFull = this.state.isGameFull;

        let clientName = this.state.playerInfo != null ? this.state.playerInfo.playerNick : "";

        let isPlayerRegistered = !!this.state.playerInfo;

        let hasPlayerResigned = this.state.playerInfo !== null && this.state.playerInfo.resigned;

        return (
            <div id="ConnectFour" className="vertical_center">  
                <Title text="Connect Four"/> 
                <Subtitle>{clientName}</Subtitle>
                <Aboard  
                    className="vertical_center" 
                    boardState={this.state.boardState} 
                    move={(col) => this.move(col)}
                    boardColor={this.state.boardColor} 
                />
                {this.state.gameMessage}
                <OptionPanel
                    isHost = {isHost}
                    JoinGame = {this.JoinGame}
                    gameState = {gameState}
                    isGameFull = {isGameFull}
                    isPlayerRegistered = {isPlayerRegistered}
                    hasPlayerResigned = {hasPlayerResigned}
                    StartGame = {() => this.invoke('StartGame')}
                    Rematch = {() => this.invoke('Rematch')}
                    Resign = {() => this.Resign()}
                />
            </div>
        )
    }
}