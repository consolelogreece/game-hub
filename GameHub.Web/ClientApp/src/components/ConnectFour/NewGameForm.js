import React, { Component } from 'react';
import ErrorMessage from '../Common/ErrorMessage';
import axios from 'axios';
import IncrementalInput from '../Common/Forms/IncrementalInput';
import FormRegion from '../Common/Forms/FormRegion';
import { Title } from '../Common/Text';

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
            errors:{}
        };
    }

    onValueChange = e => {
        this.setState({ ...this.state, errors: {...this.state.errors, [e.origin]: undefined}, roomConfig: { ...this.state.roomConfig, [e.origin] : e.value}});
    }

    CreateRoom = e =>
    {
        e.preventDefault();
        axios.post('/api/connectfour/createroom', this.state.roomConfig)
        .then(res => this.props.history.push("/connectfour/game?g=" + res.data))
        .catch(res => this.setState({errors: res.response.data}))
    }

    render() {
        return (
            <div id="c4form">
                <Title text="New Game" />
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
                    <FormRegion name="winThreshold" label={"Win Threshold"} errors={this.state.errors.winThreshold}>
                        <IncrementalInput 
                            min={2} 
                            max={30} 
                            value={this.state.roomConfig.winThreshold} 
                            onValueChange={this.onValueChange} 
                            name="winThreshold" 
                            increment={1}
                        />
                    </FormRegion>
                    <FormRegion name="nPlayersMax"  label={"Maximum Players"} errors={this.state.errors.nPlayersMax}>
                        <IncrementalInput 
                            min={2} 
                            max={8} 
                            value={this.state.roomConfig.nPlayersMax} 
                            onValueChange={this.onValueChange} 
                            name="nPlayersMax" 
                            increment={1}
                        />
                    </FormRegion>
                    {/* Only render general error message if there are no other errors. */}
                    {!!this.state.errors.general && Object.keys(this.state.errors).length === 1 && <ErrorMessage text={this.state.errors.general} />}
                    <div onClick={this.CreateRoom}>Create</div>
                </form>
            </div>
        )
    }
}
