import React, { Component } from 'react';
import axios from 'axios';

export class NewGameChess extends Component {
    constructor(props) {
        super(props);
        this.state = {
            roomConfig: {},
            errors: {}
        };
    }
    
    CreateRoom = e =>
    {
        e.preventDefault();
        axios.post('/api/chess/createroom', this.state.roomConfig)
        .then(res => this.props.history.push("chess?g=" + res.data))
        .catch(res => this.setState({errors: res.response.data}))
    }

    HandleChange(e) {
        this.setState({ ...this.state, roomConfig: { ...this.state.roomConfig, [e.target.name]: e.target.value }})
    }

    render() {
        return (
            <div>
                <button onClick={this.CreateRoom}>Create</button>
            </div>
        )
    }
}
