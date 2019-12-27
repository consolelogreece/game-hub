import React, { Component } from 'react';
import SetupBoard from './setupBoard';
import InPlayBoard from './inPlayBoard';
import GetRenderedWidthHOC from '../HigherOrder/GetRenderedWidthHOC';
import Button from '../Buttons/Standard';
import OptionPanel from '../Common/OptionPanel';

export default class Battleships extends Component {
    constructor(props) {
        super(props);

        this.state = {
            inPlay: false
        };
    }

    componentDidMount()
    {
        console.log(this.props)
    }

    GameJoined = () => {};
    JoinGame = () => {};

    render()
    {
        let DynamicBoard = GetRenderedWidthHOC(this.state.inPlay ? InPlayBoard : SetupBoard);
        let isHost, isPlayerRegistered = false;
        let hasPlayerResigned, isGameFull = false;
        let gameState = "lobby";
        

        return (
            <div>
                <DynamicBoard />
                
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