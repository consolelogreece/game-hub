import React, { Component } from 'react';

export class ConnectFour extends Component {
    constructor(props) {
        super(props);
        this.state = {
            player: "",
            column: 0,
            response: {}
        };
    }

    MakeMove()
    {
        fetch(`api/ConnectFour/MakeMove?column=${this.state.column}&player=${this.state.player}`)
            .then(response => response.json())
            .then(data => {
                this.setState({response: data})
            });
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
