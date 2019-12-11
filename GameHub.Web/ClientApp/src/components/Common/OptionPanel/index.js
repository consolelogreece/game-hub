import React from 'react';
import Button from '../../Button';

export default class optionsPanel extends React.Component
{
    state = {
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

        var result = await this.props.JoinGame();
        
        if (result.wasSuccessful)
        {
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

        let optionsPanel = <div />;

        switch (gameState)
        {
            case "lobby":
                if (!isPlayerRegistered)
                {       
                    optionsPanel = <Button onClick={this.joinGame}>Join</Button>
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