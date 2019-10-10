import React, { Component } from 'react';
import JoinGame from './JoinGame'

export default props =>
{
    let {gameState, isHost, isPlayerRegistered, playerName} = props;

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
                    <h6>it's {playerName} turn</h6>   
                </div>
            )
            break;

        case "finished":
            optionsPanel = (
                <div>
                    <h6>Game over!</h6>
                    <button onClick={() => props.Rematch()}>Re-match</button>
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