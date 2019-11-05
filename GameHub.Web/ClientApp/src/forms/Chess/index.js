import React from 'react';
import Button from '../../components/Button';
import axios from 'axios';
import './styles.css';

export default class ChessForm extends React.PureComponent
{
    CreateRoom = e =>
    {
        e.preventDefault();
        axios.post('/api/chess/createroom', {})
        .then(res => this.props.history.push("/chess/game?g=" + res.data))
        .catch(res => {
            console.log(res);
            this.props.history.push("/");
        });
    }

    render()
    {
        return (
            <div id="chessform">
                <span id="chess-new-game-form-title">Chess</span>
                <Button style={{margin: "0 auto"}} onClick={this.CreateRoom}>Create</Button>
            </div>
        )
    }
}