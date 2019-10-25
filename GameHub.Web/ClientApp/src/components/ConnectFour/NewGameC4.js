import React, { Component } from 'react';

export class NewGameC4 extends Component {
    constructor(props) {
        super(props);
        this.state = {
            roomConfig: {
                nRows:6,
                nCols: 7,
                winThreshold: 4,
                nPlayersMax: 2
            },
            message: ""
        };
    }

    componentDidMount() 
    {  
        this.props.startConnection()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :(', err))

        this.props.on("IllegalAction", res => this.setState({message: res}))
    }

    CreateRoom = () => 
    {
        this.props.invoke('CreateRoom', this.state.roomConfig)
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
                <input name="nRows" value={this.state.roomConfig.nRows} min="0" max="30" onChange={e => this.HandleChange(e)} />
                <br />
                <h6>Columns</h6>
                <input name="nCols" value={this.state.roomConfig.nCols} min="0" max="30" type="number" onChange={e => this.HandleChange(e)} />
                <br />
                <h6>Win Threshold</h6>
                <input name="winThreshold" value={this.state.roomConfig.winThreshold} min="0" max="30" type="number" onChange={e => this.HandleChange(e)} />
                <br />
                <h6>Max Players</h6>
                <input name="nPlayersMax" value={this.state.roomConfig.nPlayersMax} min="0" max="8" type="number" onChange={e => this.HandleChange(e)} />
                <br />
                {this.state.message}
                <button onClick={() => this.CreateRoom()}>Create</button>
            </div>
        )
    }
}
