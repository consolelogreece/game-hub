import React, { Component } from 'react';
import SetupBoard from './Boards/setupBoard';
import InPlayBoard from './Boards/inPlayBoard';
import OptionPanel from '../Common/OptionPanel';
import AbsoluteCenterAlign from '../Common/AbsoluteCenter';
import './styles.scss';

export default class Battleships extends Component {
    constructor(props) {
        super(props);

        this.state = {
            inPlay: false,
            gameConfiguration: {},
            playerBoardState:[[]],
            opponentBoardState: [[]],
            opponentShips: [],
            playerShips: [],
            playerInfo: null,
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
        this.mapShipOrientations(gameState.opponentSunkShips);

        this.setState({
            playerShips: gameState.playerShips, 
            opponentShips: gameState.opponentSunkShips,
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
            await this.populateGameState();
            await this.populatePlayerClientInfo();
        }
    }

    GetDynamicBoard()
    {
        let allowMoving = this.state.gameState === "lobby";

        let message = !allowMoving ? (
        <div style={{
                color:"white", 
                position: "absolute", 
                width: "100%", 
                height: "100%", 
                backgroundColor: "rgba(0,0,0,0.8)", 
                zIndex: "99"}}
            >
            <AbsoluteCenterAlign>{"Waiting for both players to join"}</AbsoluteCenterAlign>
        </div>) : "";

        
        if (this.state.playerInfo !== null && this.state.playerInfo.ready)
        {
            return <InPlayBoard width={this.props.containerWidth / 2} ships={this.state.playerShips} boardState={this.state.playerBoardState} onSquareClick={() => {}}></InPlayBoard>
        }
        else
        {
            if (allowMoving)
            {
                return <SetupBoard ships = {this.state.gameConfiguration.initialShipLayout} 
                    ReadyUp={(ships) => this.invoke("RegisterShips", ships).then(res => this.handleRegistrationResponse(res))}
                    width={this.props.containerWidth / 2}
                >{message}</SetupBoard>
            }
            else
            {
                return <InPlayBoard 
                    ships = {this.state.gameConfiguration.initialShipLayout}
                    width={this.props.containerWidth / 2}>
                    {message}
                </InPlayBoard>
            } 
        }
    }

    isHost = () =>
    {
        return this.state.playerInfo === null ? null : this.state.playerInfo.isHost;
    }

    makeMove = (row, col) =>
    {
        this.invoke('Move', {row: row, col: col});
    }

    render()
    {
        let DynamicBoard = this.GetDynamicBoard();
        let isPlayerRegistered = this.state.playerInfo !== null;
        let isHost = this.isHost();
        let hasPlayerResigned, isGameFull = false;
        let gameState = this.state.gameState;
        
        return (
            <div>
                <div id="boardsContainer">
                    {(gameState === "started"|| gameState === "finished") && <div id="opponentBoard">
                        <span style={{width:"100", textAlign:"center"}}>{"Opponents board"}</span>
                        <InPlayBoard width={this.props.containerWidth / 2} ships={this.state.opponentShips} boardState={this.state.opponentBoardState} onSquareClick={this.makeMove}/>
                    </div>}
                    <div id="playerBoard">
                        <span style={{width:"100", textAlign:"center"}}>{"Your board"}</span>
                        {DynamicBoard}
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