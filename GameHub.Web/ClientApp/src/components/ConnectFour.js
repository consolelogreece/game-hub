import React, { Component } from 'react';
import {HubConnectionBuilder} from '@aspnet/signalr'

export class ConnectFour extends Component {
    constructor(props) {
        super(props);
        this.state = {
            player: "",
            column: 0,
            response: {},
            hubConnection: null
        };
    }

    componentDidMount() {
        const hubConnection = new HubConnectionBuilder()
            .withUrl("/connectfourhub")
            .build();

        console.log(hubConnection);

        this.setState({ hubConnection }, () => {
            this.state.hubConnection
                .start()
                .then(() => console.log('Connection started!'))
                .catch(err => console.log('Error while establishing connection :(', err))

            this.state.hubConnection.on('PlayerMoved', res => {
                console.log("res recieved", res)
            });
        });
    }

    MakeMove()
    {
        this.state.hubConnection.invoke('MakeMove', this.state.column, this.state.player).catch(err => console.error(err));;
    }

    HandleChange(e)
    {
        this.setState({...this.state, [e.target.name]: e.target.value})
    }

    render()
    {
        return (
            <div>
                <h6>Column</h6>
                <input name="column" onChange={e => this.HandleChange(e)} />
                <br />
                <h6>Player</h6>
                <input name="player" onChange={e => this.HandleChange(e)} />
                <br />
                <button onClick={() => this.MakeMove()}>go</button>
                <br />
                {JSON.stringify(this.state.response)}
            </div>
        )
    }
}
