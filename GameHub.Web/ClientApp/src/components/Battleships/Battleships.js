import React, { Component } from 'react';
import SetupBoard from './Boards/setupBoard';
import InPlayBoard from './Boards/inPlayBoard';
import OptionPanel from '../Common/OptionPanel';
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
        gameState.configuration.initialShipLayout.map(s => s.orientation = s.orientation === 1 ? "horizontal" : "vertical");
        this.setState({
            playerShips: gameState.playerShips, 
            opponentShips: gameState.opponentSunkShips,
            gameConfiguration: gameState.configuration,
            playerBoardState: gameState.playerBoard,
            opponentBoardState: gameState.opponentBoard
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
        if (this.state.playerInfo !== null && this.state.playerInfo.ready)
        {
            return <InPlayBoard width={this.props.containerWidth / 2} ships={this.state.playerShips} boardState={this.state.playerBoardState} onSquareClick={() => {}}/>
        }
        else
        {
            return <SetupBoard ships = {this.state.gameConfiguration.initialShipLayout} 
                    ReadyUp={(ships) => this.invoke("RegisterShips", ships).then(res => this.handleRegistrationResponse(res))}
                    width={this.props.containerWidth / 2}
                />
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
        let gameState = "lobby";
        
        return (
            <div>
                <div id="boardsContainer">
                    <div id="opponentBoard">
                        <InPlayBoard width={this.props.containerWidth / 2} ships={this.state.opponentShips} boardState={this.state.opponentBoardState} onSquareClick={this.makeMove}/>
                    </div>
                    <div id="playerBoard">
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