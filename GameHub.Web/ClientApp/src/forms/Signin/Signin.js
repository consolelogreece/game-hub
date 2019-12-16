import React, { Component } from 'react';
import FormRegion from '../../components/Forms/FormRegion';
import Button from '../../components/Buttons/StandardWithSpinner';
import Popup from '../../components/Popup';
import StandardInput from '../../components/Forms/StandardInput';
import { timeout } from '../../utils/sleep'
import { transition_period } from './styles.scss';
import axios from 'axios';

const showFormClass = "signin-form-show";
const hideFormClass = "signin-form-hide";
const showPopupClass = "signin-popup-show";
const hidePopupClass = "signin-popup-hide";

export default class JoinGame extends Component
{   
    constructor(props)
    {
        super(props);

        this.state = {
            text:"",
            renderPopup: true,
            error: "",
            loadingStatus: "none"
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
        this.setState({...this.state, [e.target.name]: e.target.value, loadingStatus: "none"})
    }

    JoinGame = async (e) =>
    {
        e.preventDefault();

        var username = this.state.text;

        this.setState({loadingStatus: "loading"})

        //todo validation

        // prepending with "=" is necessary for binding primitives in asp net core actions. see https://blog.codenamed.nl/2015/05/12/why-your-frombody-parameter-is-always-null/
        axios.post('/api/auth/signup', "=" + username)
        .then(async res => {
            this.setState({loadingStatus: "success"});

            this.props.context.setUsername(res.data);     

            await timeout(transition_period * 3);

            this.closePopup();
        })
        .catch(res => this.setState({error: res.response.data, loadingStatus: "none"}));
    }

    render()
    {
        let renderDependentClassForm = this.state.renderPopup ? showFormClass : hideFormClass;
        let renderDependentClassPopup = this.state.renderPopup ? showPopupClass : hidePopupClass;

        let submitButtonStyles = {color: "white", margin: "0 auto"};

        let superContainerClassNames = renderDependentClassPopup;

        if (this.state.loadingStatus !== "none")
        {
            //superContainerClassNames += " blur-filter";
            submitButtonStyles.color = "#333";
        }

        return(
            <div>
                <Popup 
                    superContainerClassNames={superContainerClassNames} 
                    containerClassNames={renderDependentClassForm} 
                    popupStyles={{width:" 100%", maxWidth: "700px"}} 
                    superContainerStyles={{backgroundColor: "rgba(0,0,0, 0.5)"}}
                    onClose={this.closePopup}
                >
                    <div style={{margin: "15px auto 0px auto", textAlign: "center"}}>
                        <form onSubmit={this.JoinGame}>
                            <div style={{padding: "10px", backgroundColor: "white", borderRadius:"15px", textAlign: "left"}}>
                                <span id="signin-join-form-title">Enter your name</span>
                                <FormRegion errors={this.state.error}>
                                    <div style={{width: "100%"}}>
                                        <StandardInput name="text" value={this.state.text} onValueChange={this.HandleChange}/>
                                    </div>
                                </FormRegion>
                                <Button onClick={this.JoinGame} loadingStatus={this.state.loadingStatus}>Join</Button>
                            </div>
                        </form>
                    </div>
                </Popup>
            </div>
        )
    }
}