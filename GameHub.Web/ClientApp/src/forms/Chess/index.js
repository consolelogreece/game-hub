import React from 'react';
import Button from '../../components/Buttons/StandardWithSpinner';
import axios from 'axios';
import { timeout } from '../../utils/sleep';
import { transition_period } from './styles.scss';

export default class ChessForm extends React.PureComponent
{
    constructor(props)
    {
        super(props);

        this.state = {
            loadingStatus: "none"
        }
    }
    CreateRoom = e =>
    {
        e.preventDefault();
        axios.post('/api/chess/createroom', {})
        .then(async res => {
            this.setState({loadingStatus: "success"});

            await timeout(transition_period * 3);

            this.props.history.push("/chess/game?g=" + res.data);
        })
        .catch(res => {
            this.props.history.push("/");
        });
    }

    render()
    {
        return (
            <div id="chessform">
                <span id="chess-new-game-form-title">Chess</span>
                <Button loadingStatus={this.state.loadingStatus} style={{margin: "0 auto"}} onClick={this.CreateRoom}>Create</Button>
            </div>
        )
    }
}