import React, { Component } from 'react';
import JoinGame from './JoinGame'

export default props =>
{
    let {gameState, isHost, isPlayerRegistered} = props;

    let optionsPanel;

    switch (gameState)
    {
        case "lobby":
            optionsPanel = (
                <div>                        
                    {!isPlayerRegistered &&
                        <JoinGame 
                            title="What's your name?"
                            JoinGame={(name) => props.JoinGame(name)}
                        />
                    }
                    {isHost &&
                        <button onClick={() => props.StartGame()}>Start Game</button>    
                    }      
                </div>
            )
            break;

        case "started":
            optionsPanel = (
                <div>
                    <button onClick={() => props.Resign()}>Resign</button>
                </div>
            )
            break;

        case "finished":
            optionsPanel = (
                <div>
                    {isHost &&
                        <button onClick={() => props.Rematch()}>Re-match</button>
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