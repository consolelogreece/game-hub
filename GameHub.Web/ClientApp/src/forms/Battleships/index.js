import React from 'react';
import Button from '../../components/Buttons/StandardWithSpinner';
import axios from 'axios';
import { timeout } from '../../utils/sleep';
import { transition_period } from './styles.scss';

export default class BattleshipsForm extends React.PureComponent
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
        axios.post('/api/battleships/createroom', {})
        .then(async res => {
            this.setState({loadingStatus: "success"});

            await timeout(transition_period * 3);

            this.props.history.push("/battleships/game?g=" + res.data);
        })
        .catch(res => {
            this.props.history.push("/");
        });
    }

    render()
    {
        return (
            <div id="battleshipsform">
                <span id="battleships-new-game-form-title">Battleships</span>
                <Button loadingStatus={this.state.loadingStatus} style={{margin: "0 auto"}} onClick={this.CreateRoom}>Create</Button>
            </div>
        )
    }
}