import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr'

import Board from './Board';

export class ConnectFour extends Component {
    constructor(props) {
        super(props);
        this.state = {
            player: "",
            column: 0,
            gameId: this.props.match.params.gameId,
            gameMessage: "",
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
                .then(() => {
                    this.state.hubConnection.invoke('JoinRoom', this.state.gameId);
                })
                .catch(err => console.log('Error while establishing connection :(', err))

            this.state.hubConnection.on('PlayerMoved', res => {
                const { boardState } = this.state;

                boardState[res.row][res.col] = res.player;

                this.setState({
                    boardState: boardState,
                    gameMessage: res.message
                });
            });


        });
    }

    MakeMove()
    {
        this.state.hubConnection.invoke('MakeMove', this.state.gameId, this.state.column, this.state.player).catch(err => console.error(err));;
    }

    HandleChange(e)
    {
        this.setState({...this.state, [e.target.name]: e.target.value})
    }

    render()
    {
        return (
            <div>
                <Board boardState={this.state.boardState}/>
                <h6>Column</h6>
                <input name="column" onChange={e => this.HandleChange(e)} />
                <br />
                <h6>Player</h6>
                <input name="player" onChange={e => this.HandleChange(e)} />
                <br />
                <button onClick={() => this.MakeMove()}>go</button>
                <br />
                {this.state.gameMessage}
            </div>
        )
    }
}
