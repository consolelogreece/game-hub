import React, { Component } from 'react';
import ErrorMessage from '../Common/ErrorMessage';
import axios from 'axios';
import IncrementalInput from '../Common/Forms/IncrementalInput';
import FormRegion from '../Common/Forms/FormRegion';

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
            errors:{
                nRows: {
                    shouldRender: false,
                    message:""
                },
                nCols: {
                    shouldRender: false,
                    message:""
                },
                nPlayersMax: {
                    shouldRender: false,
                    message:""
                },
                winThreshold: {
                    shouldRender: false,
                    message:""
                }
            }
        };
    }

    onValueChange = e => {
        this.setState({ ...this.state,
             errors: {...this.state.errors, 
                [e.origin]: {message: this.state.errors[e.origin].message, shouldRender: false}}, 
                roomConfig: { ...this.state.roomConfig, [e.origin] : e.value 
                }
            }, () => {
                if (this.state.errors[e.origin].message !== "")
                {
                    setTimeout(() => {
                        this.setState({...this.state, errors:{...this.state.errors, [e.origin]: {message: "", shouldRender: false}}})
                    }, 300)
                }
            });
    
    }

    CreateRoom = e =>
    {
        e.preventDefault();
        axios.post('/api/connectfour/createroom', this.state.roomConfig)
        .then(res => this.props.history.push("connectfour?g=" + res.data))
        .catch(res => {

            let errors = this.state.errors;

            for (var key in res.response.data)
            {
                errors[key] = {
                    message: res.response.data[key],
                    shouldRender: true
                }
            }
            this.setState({errors: errors});
        })
    }

    render() {
        return (
            <div>
                <form style={{width: "70%", margin:"0 auto"}}>
                    <FormRegion label={"Rows"} error={this.state.errors.nRows}>
                        <IncrementalInput 
                            min={2} 
                            max={30} 
                            value={this.state.roomConfig.nRows} 
                            onValueChange={this.onValueChange} 
                            name="nRows" 
                            increment={1}
                        />
                    </FormRegion>
                    <FormRegion label={"Columns"} error={this.state.errors.nCols}>
                        <IncrementalInput 
                            min={2} 
                            max={30} 
                            value={this.state.roomConfig.nCols} 
                            onValueChange={this.onValueChange} 
                            name="nCols" 
                            increment={1}
                        />
                    
                    </FormRegion>
                    <FormRegion label={"Win Threshold"} error={this.state.errors.winThreshold}>
                        <IncrementalInput 
                            min={2} 
                            max={30} 
                            value={this.state.roomConfig.winThreshold} 
                            onValueChange={this.onValueChange} 
                            name="winThreshold" 
                            increment={1}
                        />
                    </FormRegion>
                    <FormRegion label={"Maximum Players"} error={this.state.errors.nPlayersMax}>
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
