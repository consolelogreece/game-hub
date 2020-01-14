import React, { Component } from 'react';
import setupBoard from './Boards/setupBoard';
import inPlayBoard from './Boards/inPlayBoard';
import OptionPanel from '../Common/OptionPanel';
import AbsoluteCenterAlign from '../Common/AbsoluteCenter';
import GetRenderedWidthHOC from '../HigherOrder/GetRenderedWidthHOC';
import withNotificationHOC from './Boards/Common/withNotificationHOC';

import './styles.scss';

let SetupBoard = GetRenderedWidthHOC(setupBoard);
let InPlayBoard = GetRenderedWidthHOC(withNotificationHOC(inPlayBoard));

export default class Battleships extends Component {
    constructor(props) {
        super(props);
        
        this.state = {
            inPlay: false,
            gameConfiguration: {},
            playerBoardState:[[]],
            opponentBoardState: [[]],
            playerShips: [],
            playerInfo: null,
            gameMessage: "",
            gameState:""
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
        ], async () => {
            let gameState = await this.fetchState();
            this.updateStateWithNewGameState(gameState);
        });

        this.props.on('RematchStarted', () => this.RematchStarted());
        
        this.initialize();   
    }

    initialize = async () =>
    {
        await this.props.startConnection();

        await this.populateGameState();
        
        await this.populatePlayerClientInfo();

        this.props.onLoadComplete();
    }

    fetchState = () =>
    {
        return this.invoke('GetGameState');
    }

    RematchStarted = async () =>
    {
        await this.populateGameState();
        await this.populatePlayerClientInfo();
    }

    GameJoined = () => {
        this.populatePlayerClientInfo()
        .then(this.populateGameState())
    };

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
        this.mapShipOrientations(gameState.configuration.initialShipLayout);
        this.mapShipOrientations(gameState.playerShips);

        this.setState({
            playerShips: gameState.playerShips, 
            gameConfiguration: gameState.configuration,
            playerBoardState: gameState.playerBoard,
            opponentBoardState: gameState.opponentBoard,
            gameState: gameState.status.status
        });
    }

    mapShipOrientations = ships =>
    {
        return ships.map(s => s.orientation = s.orientation === 1 ? "horizontal" : "vertical");
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

    handleRegistrationResponse = async res => 
    {
        if (res.wasSuccessful)
        {
            await this.populatePlayerClientInfo();
            await this.populateGameState();
        }
    }

    generateMessageNode = (allowMoving) => 
    {
        return !allowMoving ? (
            <div style={{
                    color:"white", 
                    position: "absolute", 
                    width: "100%", 
                    height: "100%", 
                    backgroundColor: "rgba(0,0,0,0.8)", 
                    zIndex: "3"}}
                >
                <AbsoluteCenterAlign>{"Waiting for both players to join"}</AbsoluteCenterAlign>
            </div>) : "";
    }

    isHost = () =>
    {
        return this.state.playerInfo === null ? null : this.state.playerInfo.isHost;
    }

    makeMove = (row, col) =>
    {
        return this.invoke('Move', {row: row, col: col});
    }

    render()
    {
        let isPlayerRegistered = this.state.playerInfo !== null;
        let isHost = this.isHost();
        let hasPlayerResigned, isGameFull = false;
        let gameState = this.state.gameState;
        let allowMoving = gameState === "lobby" || gameState === "started";
        let message = this.generateMessageNode(allowMoving);
        let DynamicBoard;

        if ((this.state.playerInfo !== null && this.state.playerInfo.ready) || !allowMoving)
        {
            DynamicBoard = <InPlayBoard key="board1" ships={this.state.playerShips} boardState={this.state.playerBoardState}>{message}</InPlayBoard>
        }
        else
        {
            DynamicBoard = <SetupBoard key="board1" ships = {this.state.gameConfiguration.initialShipLayout} 
                ReadyUp={(ships) => this.invoke("RegisterShips", ships).then(res => this.handleRegistrationResponse(res))}
            >{message}</SetupBoard>
        }
        
        return (
            <div>
                {this.props.errorMessage}
                <div id="boardsContainer">
                    <div id="playerBoard">
                        <span>{"Your board"}</span>
                        {DynamicBoard}
                    </div>
                    <div id="opponentBoard">
                        <span>{"Opponents board"}</span>
                        <InPlayBoard key="board2" ships={[]} boardState={this.state.opponentBoardState} onSquareClick={this.makeMove}>{message}</InPlayBoard>
                        {this.state.gameMessage}
                    </div>
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
            </div>
        );
    }
}