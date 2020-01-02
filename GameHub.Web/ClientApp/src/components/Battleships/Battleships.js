import React, { Component } from 'react';
import SetupBoard from './Boards/setupBoard';
import InPlayBoard from './Boards/inPlayBoard';
import GetRenderedWidthHOC from '../HigherOrder/GetRenderedWidthHOC';
import Button from '../Buttons/Standard';
import OptionPanel from '../Common/OptionPanel';
import { faThumbsDown } from '@fortawesome/free-solid-svg-icons';

export default class Battleships extends Component {
    constructor(props) {
        super(props);

        this.state = {
            inPlay: false,
            playerBoardState:[[]],
            opponentBoardState: [[]],
            opponentShips: [],
            playerShips: [],
            playerInfo: null
        };
    }

    componentDidMount()
    {
        this.props.on([
            'PlayerResigned',
            'PlayerJoined',
            'GameStarted',
            'GameOver',
            'PlayerMoved'
        ], gameState => this.updateStateWithNewGameState(gameState));

        this.props.on('RematchStarted', gameState => this.RematchStarted(gameState));
        
        this.props.startConnection();
    }

    GameJoined = () => {};
    JoinGame = () => {
        return this.invoke('JoinGame');
    }

    invoke = (destination, ...rest) =>
    {
        return this.props.invoke(destination, ...rest)
        .then(res => {
            if (res !== null && !res.wasSuccessful)
            {
                this.props.displayTimedError(res.message);
            }
            
            return res;
        })
        .catch(res => this.props.displayTimedError(res));
    }

    updateStateWithNewGameState = gameState =>
    {
       this.setState({
            playerShips: gameState.playerShips, 
            opponentShips: gameState.opponentSunkShips
        });
    }

    populatePlayerClientInfo = () =>
    {
        return this.invoke('GetClientPlayerInfo')
            .then(res => 
                {
                    if (res === undefined) return; 
                    this.setState ({
                        playerInfo: res
                    })
                })
    }

    populateGameState = () =>
    {
        return this.invoke('GetGameState')
        .then(gameState => this.updateStateWithNewGameState(gameState));
    }


    GetDynamicBoard()
    {
        if (this.state.playerInfo != null && this.state.playerInfo.ready)
        {
            
            let Board = GetRenderedWidthHOC(InPlayBoard);
            return <Board ships={this.state.playerShips} />
        }
        else
        {
            let Board = GetRenderedWidthHOC(SetupBoard);
            return <Board ships = {[
                {
                    orientation: "horizontal",
                    row:  1,
                    col:  1,
                    length: 5
                },
                {
                    orientation: "vertical",
                    row:  3 ,
                    col:  9,
                    length: 4
                },
                {
                    orientation: "horizontal",
                    row:  2,
                    col:  7,
                    length: 3
                },
                {
                    orientation: "vertical",
                    row:  5,
                    col:  3,
                    length: 3
                },
                {
                    orientation: "horizontal",
                    row:  9,
                    col:  7,
                    length: 2
                }]} ReadyUp={(ships) => this.invoke("RegisterShips", ships).then(this.populateGameState).then(this.populatePlayerClientInfo)}/>
        }
    }

    render()
    {
        let DynamicBoard = this.GetDynamicBoard();
        let OpponentsBoard = GetRenderedWidthHOC(InPlayBoard);
        let isHost, isPlayerRegistered = false;
        let hasPlayerResigned, isGameFull = false;
        let gameState = "lobby";
        
        return (
            <div>
                <div style={{display: "flex"}}>
                    {DynamicBoard}
                    <OpponentsBoard ships={this.state.opponentShips}/>
                </div>
                
                <OptionPanel
                    isHost = {isHost}
                    JoinGame = {this.JoinGame}
                    GameJoined = {this.GameJoined}
                    gameState = {gameState}
                    isGameFull = {isGameFull}
                    isPlayerRegistered = {isPlayerRegistered}
                    hasPlayerResigned = {hasPlayerResigned}
                    StartGame = {() => this.invoke('StartGame')}
                    Rematch = {() => this.invoke('Rematch')}
                    Resign = {() => this.Resign()}
                />
                <Button onClick={() => this.setState({inPlay: !this.state.inPlay})}>toggle board</Button>
            </div>
        );
    }
}