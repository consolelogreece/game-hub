import React, { Component } from 'react';
import FormRegion from '../../components/Forms/FormRegion';
import Button from '../../components/Button';
import Popup from '../../components/Popup';
import StandardInput from '../../components/Forms/StandardInput';
import { timeout } from '../../utils/sleep'
import { transition_period } from './styles.scss';
import axios from 'axios';

const showFormClass = "option-panel-form-show";
const hideFormClass = "option-panel-form-hide";
const showPopupClass = "option-panel-popup-show";
const hidePopupClass = "option-panel-popup-hide";

export default class JoinGame extends Component
{   
    constructor(props)
    {
        super(props);

        this.state = {
            text:"",
            renderPopup: true,
            error: ""
        }
    }

    closePopup = async () =>
    {
        this.setState({renderPopup: false});
        
        await timeout(transition_period);

        this.props.toggle();
    }

    HandleChange = e =>
    {
        this.setState({...this.state, [e.target.name]: e.target.value})
    }

    JoinGame = (e) =>
    {
        e.preventDefault();

        var username = this.state.text;

        //todo validation

        // prepending with "=" is necessary for binding primitives in asp net core actions. see https://blog.codenamed.nl/2015/05/12/why-your-frombody-parameter-is-always-null/
        axios.post('/api/auth/signup', "=" + username)
        .then(X => console.log(X))
        .catch(X => console.log(X.message) /*this.setState({error: X})*/);
    }

    render()
    {
        let renderDependentClassForm = this.state.renderPopup ? showFormClass : hideFormClass;
        let renderDependentClassPopup = this.state.renderPopup ? showPopupClass : hidePopupClass;

        return(
            <div>
                <Popup 
                    superContainerClassNames={renderDependentClassPopup} 
                    containerClassNames={renderDependentClassForm} 
                    popupStyles={{width:" 100%", maxWidth: "700px"}} 
                    superContainerStyles={{backgroundColor: "rgba(0,0,0, 0.5)"}}
                    onClose={this.closePopup}
                >
                    <div style={{margin: "15px auto 0px auto", textAlign: "center"}}>
                        <form onSubmit={this.JoinGame}>
                            <div style={{padding: "10px", backgroundColor: "white", borderRadius:"15px", textAlign: "left"}}>
                                <span id="option-panel-join-form-title">Enter your name</span>
                                <FormRegion errors={this.state.error}>
                                    <StandardInput name="text" value={this.state.text} onValueChange={this.HandleChange}/>
                                </FormRegion>
                                <Button style={{margin: "0 auto"}} onClick={this.JoinGame}>Join</Button>
                            </div>
                        </form>
                    </div>
                </Popup>
            </div>
        )
    }
}