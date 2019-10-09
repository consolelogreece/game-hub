﻿import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';
import './ConnectFour.css';
import JoinGame from '../Common/JoinGame'
import Title from '../Common/Title'
import ResizeWithWindowHOC from '../Common/GetRenderedWidthHOC';

import Board from './Board';

export class ConnectFour extends Component {
    constructor(props) {
        super(props);

        let gameId = this.props.match.params.gameId;

        this.state = {
            playerNick: "",
            playerId: "",
            joined:false,
            column: 0,
            gameId: gameId,
            isGameCreator: false,
            gameMessage: "",
            playerTurn: "",
            gameState: "lobby",
            hubConnection: null,
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

        hubConnection.on('PlayerMoved', res => 
        {
            var playerTurn = res.nextTurnPlayer.id == this.state.playerId ? "your" : res.nextTurnPlayer.playerNick;
            this.setState({
                boardState: res.boardState,
                gameMessage: res.message,
                playerTurn: playerTurn
            });
        });
        
        hubConnection.on('RoomDoesntExist', res => {
            this.props.history.push("/connectfour/createroom")
        });
        
        hubConnection.on('IllegalAction', res => {
            this.setState({
                gameMessage: res
            });
        });
        
        hubConnection.on('PlayerWon', player => {
            this.setState({
                gameMessage: `${player.playerNick} won!`,
                gameState: "finished"
            });
        });
        
        hubConnection.on('GameStarted', gameState => {
            this.setState({
                gameState: "started",
                playerTurn: gameState.nextTurnPlayer.playerNick
            });
        });
        
        this.setState({ boardState : board, hubConnection }, () => {
            this.state.hubConnection
            .start()
            .then(() => {
                this.state.hubConnection.invoke('JoinRoom', this.state.gameId)
                .then(res => {
                    this.PopulateClientPlayerInfo();
                })
                .then(res => {
                    this.PopulateGameState();   
                })
                        .catch(res => console.log(res))
                    })
                    .catch(err => console.log('Error while establishing connection :(', err))
            });
    }

    MakeMove(col)
    {
        this.state.hubConnection.invoke('MakeMove', this.state.gameId, col).catch(err => console.error(err));;
    }
    
    HandleChange(e)
    {
        this.setState({...this.state, [e.target.name]: e.target.value})
    } 
    
    JoinGame(name) {
        this.state.hubConnection.invoke('JoinGame', this.state.gameId, name)
        .then(res =>  
            this.PopulateClientPlayerInfo()
        )
        .catch(() => this.setState({gameMessage: "oopsie daisy"}));
    }

    StartGame()
    {
        this.state.hubConnection.invoke('StartGame', this.state.gameId).catch(res => this.setState({gameMessage:res}));
    }

    PopulateGameState()
    {
        this.state.hubConnection.invoke('GetGameState', this.state.gameId)
        .then(res => 
            {
                if (res == null) return; 
                var playerTurn = "";
                if (res.nextTurnPlayer != null) res.nextTurnPlayer.id == this.state.playerId ? "your" : res.nextTurnPlayer.playerNick;
                this.setState({
                    gameState: res.status,
                    boardState: res.boardState,
                    playerTurn: playerTurn
                })
        })

        this.forceUpdate();
    }

    PopulateClientPlayerInfo()
    {
        return this.state.hubConnection.invoke('GetClientPlayerInfo', this.state.gameId)
            .then(res => 
                {
                    if (res == null) return; 
                    this.setState ({
                        joined: res != null,
                        playerNick: res.playerNick,
                        playerId: res.id,
                        isGameCreator: res.isHost
                    })
                })
    }

    render()
    {
        let optionsPanel;

        switch (this.state.gameState)
        {
            case "lobby":
                optionsPanel = (
                    <div>                        
                        {!this.state.joined &&
                            <JoinGame 
                                title="What's your name?"
                                JoinGame={(name) => this.JoinGame(name)}
                            />
                        }
                        {this.state.isGameCreator &&
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

        if (!this.state.joined && this.state.gameState != "lobby")
        {
            optionsPanel = (
                <div>
                </div>
            )
        }
        
        var Aboard = ResizeWithWindowHOC(Board);

        return (
            <div id="ConnectFour" className="vertical_center">  
                <Title text="Connect Four"/> 
                {this.state.playerNick} <br />
                <Aboard  
                    className="vertical_center" 
                    boardState={this.state.boardState} 
                    makeMove={(col) => this.MakeMove(col)}
                    boardColor={this.state.boardColor} 
                />
                {this.state.gameMessage}
                {optionsPanel}
            </div>
        )
    }
}
