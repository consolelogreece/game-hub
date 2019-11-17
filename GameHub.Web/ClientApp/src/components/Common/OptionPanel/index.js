import React from 'react';
import Popup from '../../Popup';
import FormRegion from '../../Forms/FormRegion';
import Button from '../../Button';
import StandardInput from '../../Forms/StandardInput';
import { transition_period } from './styles.scss';
import { timeout } from '../../../utils/sleep';

const showFormClass = "option-panel-form-show";
const hideFormClass = "option-panel-form-hide";
const showPopupClass = "option-panel-popup-show";
const hidePopupClass = "option-panel-popup-hide";

export default class optionsPanel extends React.Component
{
    state = {
        username: "",
        displayJoin: false,
        renderPopup: false,
        gameState: "lobby",
        error: "",
        renderCount: 0
    }

    closePopup = async () =>
    {
        this.setState({renderPopup: false}, () => console.log("RENDER POPUP SET TO FALSE"));
        
        await timeout(transition_period);

        this.setState({displayJoin: false, error: undefined}, () => console.log("DISPLAY JOIN SET TO FALSE"));
    }

    HandleChange = e =>
    {
        this.setState({...this.state, error:"", [e.target.name]: e.target.value})
    }

    joinGame = async e =>
    {
        e.preventDefault();

        var result = await this.props.JoinGame(this.state.username); // issue here
        
        if (result.wasSuccessful)
        {
            await this.closePopup();

            this.props.GameJoined();
        }
        else
        {
            this.setState({error: result.message})
        }
    }

    componentDidUpdate()
    {

    }

    toggleJoinForm = () => 
    {
        this.setState({displayJoin: !this.state.displayJoin, renderPopup: true, error: undefined});
    }

    render()
    {
        let {gameState, isHost, isPlayerRegistered, isGameFull, hasPlayerResigned} = this.props;

        let renderDependentClassForm = this.state.renderPopup ? showFormClass : hideFormClass;
        let renderDependentClassPopup = this.state.renderPopup ? showPopupClass : hidePopupClass;

        let shouldRenderJoinPopup = !isGameFull || this.state.displayJoin || this.state.renderPopup;

        let optionsPanel = <div />;

        switch (gameState)
        {
            case "lobby":
                if (!isPlayerRegistered && shouldRenderJoinPopup)
                {
                    if (this.state.displayJoin)
                    {
                        optionsPanel = (
                            <div>
                                <Popup 
                                    superContainerClassNames={renderDependentClassPopup} 
                                    containerClassNames={renderDependentClassForm} 
                                    popupStyles={{width:" 100%", maxWidth: "700px"}} 
                                    superContainerStyles={{backgroundColor: "rgba(0,0,0, 0.5)"}}
                                    onClose={this.closePopup}
                                >
                                    <form onSubmit={this.joinGame}>
                                        <div style={{padding: "10px", backgroundColor: "white", borderRadius:"15px", textAlign: "left"}}>
                                            <span id="option-panel-join-form-title">Enter your name</span>
                                            <FormRegion errors={this.state.error}>
                                                <StandardInput name="username" value={this.state.username} onValueChange={this.HandleChange}/>
                                            </FormRegion>
                                            <Button style={{margin: "0 auto"}} onClick={this.joinGame}>Join</Button>
                                        </div>
                                    </form>
                                </Popup>
                                <Button onClick={this.renderPopup}>Join</Button>
                            </div>
                        )
                    }
                    else
                    {
                        optionsPanel = <Button onClick={this.toggleJoinForm}>Join</Button>
                    }
                }
                else if(isHost)
                    {
                        optionsPanel = (
                            <button onClick={() => this.props.StartGame()}>Start Game</button>   
                        )
                    }
                break;

            case "started":
                optionsPanel = (
                    <div>
                    {!!isPlayerRegistered && !hasPlayerResigned && <button onClick={() => this.props.Resign()}>Resign</button>}
                    </div>
                )
                break;

            case "finished":
                optionsPanel = (
                    <div>
                        {isHost &&
                            <button onClick={() => this.props.Rematch()}>Re-match</button>
                        }
                    </div>
                )
                break;

            default:
                optionsPanel = (
                <div>
                </div>
            )
        }

        return optionsPanel;
    }
}