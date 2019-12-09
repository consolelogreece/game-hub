import React from 'react';
import Button from '../../Button';

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

    HandleChange = e =>
    {
        this.setState({...this.state, error:"", [e.target.name]: e.target.value})
    }

    joinGame = async e =>
    {
        e.preventDefault();

        var result = await this.props.JoinGame(this.state.username);
        
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


    render()
    {
        let {gameState, isHost, isPlayerRegistered, isGameFull, hasPlayerResigned} = this.props;

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
                                {/* todo: have this toggle choose username if user not signed in. maybe have a spectate option that just hides this option */}
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
                            <Button onClick={() => this.props.StartGame()}>Start Game</Button>   
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