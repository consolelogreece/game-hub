import React, { Component } from 'react';

export class NewGameChess extends Component {
    constructor(props) {
        super(props);
        this.state = {
            roomConfig: {}
        };
    }

    componentDidMount() 
    {  
        this.props.startConnection()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :(', err))
    }

    CreateRoom = () => {
        this.props.invoke('CreateRoom')
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
