import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr'

export class NewGameC4 extends Component {
    constructor(props) {
        super(props);
        this.state = {
            hubConnection: null,
            roomConfig: {
                nRows:6,
                nCols: 7,
                winThreshold: 4,
                nPlayersMax: 2
            }
        };
    }

    componentDidMount() {
        const hubConnection = new HubConnectionBuilder()
            .withUrl("/connectfourhub")
            .build();
            
        this.setState({ hubConnection }, () => {
            this.state.hubConnection
                .start()
                .then(() => console.log('Connection started!'))
                .catch(err => console.log('Error while establishing connection :(', err))
        });
    }

    CreateRoom = () => {
        console.log(this.state)
        this.state.hubConnection.invoke('CreateRoom', this.state.roomConfig)
            .then(gameId => this.props.history.push(gameId))
            .catch(err => console.error(err));
    }

    HandleChange(e) {
        this.setState({ ...this.state, roomConfig: { ...this.state.roomConfig, [e.target.name]: e.target.value }})
    }

    render() {
        return (
            <div>
                <h6>Rows</h6>
                <input name="nRows" value={this.state.roomConfig.nRows} onChange={e => this.HandleChange(e)} />
                <br />
                <h6>Columns</h6>
                <input name="nCols" value={this.state.roomConfig.nCols} onChange={e => this.HandleChange(e)} />
                <br />
                <h6>Win Threshold</h6>
                <input name="winThreshold" value={this.state.roomConfig.winThreshold} onChange={e => this.HandleChange(e)} />
                <br />
                <h6>Max Players</h6>
                <input name="nPlayersMax" value={this.state.roomConfig.nPlayersMax} onChange={e => this.HandleChange(e)} />
                <br />
                <button onClick={() => this.CreateRoom()}>Create</button>
            </div>
        )
    }
}
