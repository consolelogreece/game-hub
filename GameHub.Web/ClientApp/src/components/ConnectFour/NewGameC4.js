﻿import React, { Component } from 'react';
import  ErrorMessage from '../Common/ErrorMessage';
import axios from 'axios';

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
            errors:{}
        };
    }

    HandleChange(e) {
        this.setState({ ...this.state, roomConfig: { ...this.state.roomConfig, [e.target.name]: e.target.value }})
    }

    CreateRoom = e =>
    {
        e.preventDefault();
        axios.post('/api/connectfour/createroom', this.state.roomConfig)
        .then(res => this.props.history.push("connectfour?g=" + res.data))
        .catch(res => this.setState({errors: res.response.data}))
    }

    render() {
        return (
            <div>
                <form onSubmit={this.CreateRoom}>
                    <h6>Rows</h6>
                    {!!this.state.errors.nRows && <ErrorMessage text={this.state.errors.nRows} />}
                    <input name="nRows" value={this.state.roomConfig.nRows} min="2" max="30" type="number" onChange={e => this.HandleChange(e)} />
                    <br />
                    <h6>Columns</h6>
                    {!!this.state.errors.nCols && <ErrorMessage text={this.state.errors.nCols} />}
                    <input name="nCols" value={this.state.roomConfig.nCols} min="2" max="30" type="number" onChange={e => this.HandleChange(e)} />
                    <br />
                    <h6>Win Threshold</h6>
                    {!!this.state.errors.winThreshold && <ErrorMessage text={this.state.errors.winThreshold} />}
                    <input name="winThreshold" value={this.state.roomConfig.winThreshold} min="2" max="30" type="number" onChange={e => this.HandleChange(e)} />
                    <br />
                    <h6>Max Players</h6>
                    {!!this.state.errors.nPlayersMax && <ErrorMessage text={this.state.errors.nPlayersMax} />}
                    <input name="nPlayersMax" value={this.state.roomConfig.nPlayersMax} min="2" max="8" type="number" onChange={e => this.HandleChange(e)} />
                    <br />
                    {/* Only render general error message if there are no other errors. */}
                    {!!this.state.errors.general && Object.keys(this.state.errors).length === 1 && <ErrorMessage text={this.state.errors.general} />}
                    <button>Create</button>
                </form>
            </div>
        )
    }
}
