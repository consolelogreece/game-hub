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
            inPlay: false,
            playerBoardState:[[]],
            opponentBoardState: [[]]
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

        console.log("hmmk")
        
        this.props.startConnection().then(x => console.log(x));
    }

    GameJoined = () => {console.log("yo ho ho")};
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
        // var message = this.generateGameMessageFromGameState(gameState);
        // var isGameFull = this.isGameFull(gameState);
        // this.setState({
        //     boardState: gameState.boardState,
        //     gameState: gameState.status.status,
        //     playerTurn: this.getTurnIndicator(gameState.currentTurnPlayer),
        //     isGameFull: isGameFull,
        //     gameMessage: message
        // })

        console.log(gameState);
    }

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