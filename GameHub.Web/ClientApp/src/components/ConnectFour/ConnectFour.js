import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';
import './ConnectFour.css';
import { Title, Subtitle } from '../Common/Text';
import ResizeWithContainerHOC from '../Common/GetRenderedWidthHOC';
import OptionPanel from '../Common/OptionPanel';

import Board from './Board';

export class ConnectFour extends Component {
    constructor(props) {
        super(props);

        let gameId = this.props.match.params.gameId;

        this.state = {
            column: 0,
            gameId: gameId,
            gameMessage: "",
            playerTurn: "",
            gameState: "lobby",
            hubConnection: null,
            playerInfo: null,
            boardState: [[]],
            boardColor: "#0e3363"
        };
    }

    componentDidMount() {
        let board = [];
        
        for (let i = 0; i < this.state.boardState.length; i++) {
            let row = [];
            for (let j = 0; j < this.state.boardState[i].length; j++) {
                row.push("white");
            }
            board.push(row);
        }

        const hubConnection = new HubConnectionBuilder()
        .withUrl("/connectfourhub", {accessTokenFactory: () => "testing"})
        .build();

        hubConnection.on('PlayerMoved', gameState => this.updateStateWithNewGameState(gameState));
        
        hubConnection.on('RoomDoesntExist', res => {
            this.props.history.push("/connectfour/createroom")
        });

        hubConnection.on('GameOver', gameState => this.updateStateWithNewGameState(gameState));
        
        hubConnection.on('IllegalAction', res => {
            this.setState({
                gameMessage: res
            });
        });
        
        hubConnection.on('GameStarted', gameState => this.updateStateWithNewGameState(gameState));
        
        this.setState({ boardState : board, hubConnection }, () => {
            this.state.hubConnection
            .start()
            .then(() => {
                this.state.hubConnection.invoke('JoinRoom', this.state.gameId)
                .then(res => {
                    this.populatePlayerClientInfo();
                })
                .then(res => {
                    this.populateGameState();   
                })
                        .catch(res => console.log(res))
                    })
                    .catch(err => console.log('Error while establishing connection :(', err))
            });
    }

    MakeMove = col =>
    {
        this.state.hubConnection.invoke('MakeMove', this.state.gameId, col).catch(err => console.error(err));;
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

    Rematch = () =>
    {
        this.state.hubConnection.invoke('Rematch', this.state.gameId);
    }

    Resign = () =>
    {
        this.state.hubConnection.invoke('Resign', this.state.gameId);
    }

    populateGameState = () =>
    {
        this.state.hubConnection.invoke('GetGameState', this.state.gameId)
        .then(gameState => this.updateStateWithNewGameState(gameState));
    }

    updateStateWithNewGameState = gameState =>
    {
        var message = this.generateGameMessageFromGameState(gameState);
        this.setState({
            boardState: gameState.boardState,
            gameState: gameState.status.status,
            playerTurn: this.getTurnIndicator(gameState.currentTurnPlayer),
            gameMessage: message
        })
    }

    getTurnIndicator = player => 
    {
        console.log("player", player)
        if (this.state.playerInfo === null) return "";

        return player.id === this.state.playerInfo.id ? "your" : (player.playerNick + "'s");
    }

    generateGameMessageFromGameState = gameState =>
    {
        let message = "";

        let {status, endReason} = gameState.status;

        if (status === "finished") message = endReason;

        if (status === "lobby")
        {
            if (this.state.playerInfo === null)
            {
                message = "Please enter your name"
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
        return this.state.hubConnection.invoke('GetClientPlayerInfo', this.state.gameId)
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

    render()
    {
        var Aboard = ResizeWithContainerHOC(Board);

        let gameState = this.state.gameState;

        let isHost = this.isHost();

        let clientName = this.state.playerInfo != null ? this.state.playerInfo.playerNick : "";

        let isPlayerRegistered = !!this.state.playerInfo;

        return (
            <div id="ConnectFour" className="vertical_center">  
                <Title text="Connect Four"/> 
                <Subtitle text={clientName} />
                <Aboard  
                    className="vertical_center" 
                    boardState={this.state.boardState} 
                    makeMove={(col) => this.MakeMove(col)}
                    boardColor={this.state.boardColor} 
                />
                {this.state.gameMessage}
                <OptionPanel
                    isHost = {isHost}
                    JoinGame = {this.JoinGame}
                    gameState = {gameState}
                    isPlayerRegistered = {isPlayerRegistered}
                    StartGame = {this.StartGame}
                    Rematch = {this.Rematch}
                    Resign = {this.Resign}
                />
                <button onClick={() => console.log(this.state)}>log state</button>
            </div>
        )
    }
}