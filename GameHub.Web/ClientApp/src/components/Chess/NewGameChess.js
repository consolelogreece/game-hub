import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr'

export class NewGameChess extends Component {
    constructor(props) {
        super(props);
        this.state = {
            hubConnection: null,
            roomConfig: {
            }
        };
    }

    componentDidMount() {
        const hubConnection = new HubConnectionBuilder()
            .withUrl("/chesshub")
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
        this.state.hubConnection.invoke('CreateRoom')
            .then(gameId => this.props.history.push(gameId))
            .catch(err => console.error(err));
    }

    HandleChange(e) {
        this.setState({ ...this.state, roomConfig: { ...this.state.roomConfig, [e.target.name]: e.target.value }})
    }

    render() {
        return (
            <div>
                <button onClick={() => this.CreateRoom()}>Create</button>
            </div>
        )
    }
}
