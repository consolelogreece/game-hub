import React from 'react';
import Popup from '../../Popup';
import FormRegion from '../../Forms/FormRegion';
import Button from '../../Button';
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
        renderPopup: false
    }

    closePopup = async () =>
    {
        this.setState({renderPopup: false});

        await timeout(transition_period);

        this.setState({displayJoin: false});

        return new Promise(resolve => resolve())
    }

    HandleChange(e)
    {
        this.setState({...this.state, [e.target.name]: e.target.value})
    }

    joinGame = async e =>
    {
        e.preventDefault();

        await this.closePopup();

        this.props.JoinGame(this.state.username);
    }

    toggleJoinForm = () => 
    {
        this.setState({displayJoin: !this.state.displayJoin, renderPopup: true});
    }

    render()
    {
        let {gameState, isHost, isPlayerRegistered, isGameFull, hasPlayerResigned} = this.props;

        let renderDependentClassForm = this.state.renderPopup ? showFormClass : hideFormClass;
        let renderDependentClassPopup = this.state.renderPopup ? showPopupClass : hidePopupClass;

        let optionsPanel = <div />;

        switch (gameState)
        {
            case "lobby":
                if (!isPlayerRegistered && !isGameFull)
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
                                    <form>
                                        <div style={{padding: "10px", backgroundColor: "white", borderRadius:"15px"}}>
                                            <FormRegion label="Please enter your name">
                                                <input  
                                                    name="username" 
                                                    value={this.state.username} 
                                                    onChange={e => this.HandleChange(e)}
                                                    style={{
                                                        width:"100%",
                                                        border: "2px solid #aaa",
                                                        borderRadius: "4px",
                                                        margin: "8px 0",
                                                        padding: "8px"
                                                    }}
                                                />
                                                <Button style={{margin: "0 auto"}} onClick={this.joinGame}>Join</Button>
                                            </FormRegion>
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