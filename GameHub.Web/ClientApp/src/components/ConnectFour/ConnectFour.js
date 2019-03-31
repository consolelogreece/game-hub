import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr'

import Board from './Board';

export class ConnectFour extends Component {
    constructor(props) {
        super(props);
        this.state = {
            playerNick: "",
            playerColor: "",
            column: 0,
            gameId: this.props.match.params.gameId,
            gameMessage: "",
            playerTurn: "",
            gameState: "lobby",
            hubConnection: null,
            boardState: [],
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
            .withUrl("/connectfourhub")
            .build();


        this.setState({ hubConnection }, () => {
            this.state.hubConnection
                .start()
                .catch(err => console.log('Error while establishing connection :(', err))

            this.state.hubConnection.on('PlayerMoved', res => {
                const { boardState } = this.state;
                boardState[res.row][res.col] = res.playerColor;

                let gameState = res.DidMoveWin ? "complete" : this.state.gameState;

                this.setState({
                    boardState: boardState,
                    gameMessage: res.message,
                    playerTurn: `Turn: ${res.playerNick}`,
                    gameState: gameState
                });
            });

            this.state.hubConnection.on('GameStarted', firstPlayerName => {
                this.setState({
                    gameState: "inProgress",
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

    JoinRoom()
    {
        this.state.hubConnection.invoke('JoinRoom', this.state.gameId, this.state.playerNick, this.state.playerColor);
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
                        <h6>Player Color</h6>
                        <input name="playerColor" value={this.state.playerColor} onChange={e => this.HandleChange(e)} />
                        <h6>Player Nickname</h6>
                        <input name="playerNick" value={this.state.playerNick} onChange={e => this.HandleChange(e)} />
                        <br />
                        <button onClick={() => this.JoinRoom()}>Join</button>
                        <br/>
                        <button onClick={() => this.StartGame()}>StartGame</button>    
                    </div>
                )
                break;

            case "inProgress":
                optionsPanel = (
                    <div>
                        <h6> Column </h6>
                        <input name="column" value={this.state.column}  onChange={e => this.HandleChange(e)} />
                        <button onClick={() => this.MakeMove()}>Place token</button>
                    </div>
                )
                break;

            case "complete":
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
                <Board boardState={this.state.boardState}/>
                {optionsPanel}
            </div>
        )
    }
}
