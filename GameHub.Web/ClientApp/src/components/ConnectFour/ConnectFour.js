import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr'

import Board from './Board';

export class ConnectFour extends Component {
    constructor(props) {
        super(props);

        let gameId = this.props.match.params.gameId;

        this.state = {
            playerNick: "",
            joined:false,
            column: 0,
            gameId: gameId,
            isGameCreator: false,
            gameMessage: "",
            playerTurn: "",
            gameState: "lobby",
            hubConnection: null,
            boardState: [[]],
            nRows: 6,
            nCols: 7
        };
    }

    componentDidMount() {
        let board = [];

        for (let i = 0; i < this.state.nRows; i++) {
            let row = [];
            for (let j = 0; j < this.state.nCols; j++) {
                row.push("white");
            }
            board.push(row);
        }

        this.setState({boardState: board})

        const hubConnection = new HubConnectionBuilder()
            .withUrl("/connectfourhub", {accessTokenFactory: () => "testing"})
            .build();

        this.setState({ hubConnection }, () => {
            this.state.hubConnection
                .start()
                .then(() => {
                    this.state.hubConnection.invoke('JoinRoom', this.state.gameId)
                        .then(res => {
                            console.log(res);
                            this.setState({
                                gameState: res.status,
                                playerNick: res.registeredNick,
                                joined: res.isPlayerRegistered,
                                isGameCreator: res.isGameCreator,
                                boardState: res.boardState
                            })
                        })
                        .catch(res => console.log(res))
                })
                .catch(err => console.log('Error while establishing connection :(', err))

            this.state.hubConnection.on('PlayerMoved', res => {
                console.log(res);
                this.setState({
                    boardState: res.boardState,
                    gameMessage: res.message,
                    playerTurn: `Turn: ${res.playerNick}`
                });
            });

            this.state.hubConnection.on('InvalidMove', res => {
                this.setState({
                    gameMessage: res.message
                });
            });

            this.state.hubConnection.on('PlayerWon', playerNick => {
                this.setState({
                    gameMessage: `${playerNick} won!`,
                    gameState: "complete"
                });
            });

            this.state.hubConnection.on('GameStarted', firstPlayerName => {
                this.setState({
                    gameState: "started",
                    playerTurn: `Turn: ${firstPlayerName}`
                });
            });
        });
    }

    MakeMove()
    {
        this.state.hubConnection.invoke('MakeMove', this.state.gameId, this.state.column).catch(err => console.error(err));;
    }

    HandleChange(e)
    {
        this.setState({...this.state, [e.target.name]: e.target.value})
    } 

    JoinGame() {
        this.state.hubConnection.invoke('JoinGame', this.state.gameId, this.state.playerNick)
            .then(res => this.setState({
                joined: true,
                boardState: res.boardState
            }))
            .catch(() => this.setState({gameMessage: "oopsie daisy"}));
    }

    StartGame()
    {
        this.state.hubConnection.invoke('StartGame', this.state.gameId);
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
                            <div>
                                <h6>Choose your nickname</h6>
                                <input name="playerNick" value={this.state.playerNick} onChange={e => this.HandleChange(e)} />
                                <br /> 
                                <button onClick={() => this.JoinGame()}>Join</button>
                            </div>
                        }
                        {this.state.isGameCreator &&
                            <button onClick={() => this.StartGame()}>StartGame</button>    
                        }
                       
                    </div>
                )
                break;

            case "started":
                optionsPanel = (
                    <div>
                        <h6> Column </h6>
                        <input name="column" value={this.state.column}  onChange={e => this.HandleChange(e)} />
                        <button onClick={() => this.MakeMove()}>Place token</button>
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
                {this.state.playerNick} <br />
                <Board boardState={this.state.boardState}/>
                {optionsPanel}
            </div>
        )
    }
}
