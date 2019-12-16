import React, { Component } from 'react';
import axios from 'axios';
import IncrementalInput from '../../components/Forms/IncrementalInput';
import FormRegion from '../../components/Forms/FormRegion';
import Button from '../../components/Buttons/StandardWithSpinner';
import Tooltip from '../../components/Tooltip';
import { timeout } from '../../utils/sleep';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faQuestionCircle } from '@fortawesome/free-solid-svg-icons';
import { transition_period } from './styles.scss';

export default class NewGameForm extends Component {
    constructor(props) {
        super(props);
        this.state = {
            roomConfig: {
                nRows:6,
                nCols: 7,
                winThreshold: 4,
                nPlayersMax: 2
            },
            errors:{},
            loadingStatus: "none"
        };
    }

    onValueChange = e => {
        this.setState({ ...this.state, errors: {...this.state.errors, [e.origin]: undefined}, roomConfig: { ...this.state.roomConfig, [e.origin] : e.value}});
    }

    CreateRoom = e =>
    {
        e.preventDefault();
        axios.post('/api/connectfour/createroom', this.state.roomConfig)
        .then(async res => {
            this.setState({loadingStatus: "success"});

            await timeout(transition_period * 3);
            
            this.props.history.push("/connectfour/game?g=" + res.data);
        })
        .catch(res => this.setState({errors: res.response.data}))
    }

    render() {
        return (
            <div id="c4form">
                <span id="c4-new-game-form-title">Connect Four</span>
                <form style={{margin:"0 auto"}}>
                    <FormRegion name="nRows" label={"Rows"} errors={this.state.errors.nRows}>
                        <IncrementalInput 
                            min={2} 
                            max={30} 
                            value={this.state.roomConfig.nRows} 
                            onValueChange={this.onValueChange} 
                            name="nRows" 
                            increment={1}
                        />
                    </FormRegion>
                    <FormRegion name="nCols" label={"Columns"} errors={this.state.errors.nCols}>
                        <IncrementalInput 
                            min={2} 
                            max={30} 
                            value={this.state.roomConfig.nCols} 
                            onValueChange={this.onValueChange} 
                            name="nCols" 
                            increment={1}
                        />
                    </FormRegion>
                    <FormRegion name="winThreshold" label={
                    <span>
                        Win Threshold <Tooltip parentTransitionPeriod={transition_period} text={<FontAwesomeIcon icon={faQuestionCircle} />} tooltip={"Number of tokens needed in a row to win"}/>
                    </span>
                    } errors={this.state.errors.winThreshold}>
                        <IncrementalInput 
                            min={2} 
                            max={30} 
                            value={this.state.roomConfig.winThreshold} 
                            onValueChange={this.onValueChange} 
                            name="winThreshold" 
                            increment={1}
                        />
                    </FormRegion>
                    <FormRegion name="nPlayersMax" label={"Maximum Players"} errors={this.state.errors.nPlayersMax}>
                        <IncrementalInput 
                            min={2} 
                            max={8} 
                            value={this.state.roomConfig.nPlayersMax} 
                            onValueChange={this.onValueChange} 
                            name="nPlayersMax" 
                            increment={1}
                        />
                    </FormRegion>
                    <Button loadingStatus={this.state.loadingStatus} style={{margin: "0 auto"}} onClick={this.CreateRoom}>Create</Button>
                </form>
            </div>
        )
    }
}
