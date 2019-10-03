import React, { Component } from "react";
import Chessboard from "chessboardjsx";
import { HubConnectionBuilder } from '@aspnet/signalr';

export default class Chess extends Component {
    constructor(props) {
        super(props);
        this.state = {
            hubConnection: null,
            fen: "empty",
            gameId: props.match.params.gameId,
            legalMoves: {},
            squareStyles: {},
            pieceSquare:""
        }
    }

    componentDidMount() {
        let gameState = [];

        const hubConnection = new HubConnectionBuilder()
        .withUrl("/chesshub", {accessTokenFactory: () => "testing"})
        .build();

        this.setState({ hubConnection }, () => {
            this.state.hubConnection
            .start()
            .then(() => {
                this.state.hubConnection.invoke('GetGameState', this.state.gameId)
                .then(res => {
                    this.setState({fen: res.boardStateFen})
                })
                .then(this.state.hubConnection.invoke('GetMoves', this.state.gameId)
                .then(res => this.mapMoves(res)))
            })
        });
    }

    mapMoves(moves)
    {
        if (moves == null) return;

        let files = "abcdefgh"

        let map = {};

        let mappedMoves = moves.forEach(el => {
            let key = `${files[el.originalPosition.file]}${el.originalPosition.rank}`;

            if (!(key in map))
            {
                map[key] = [];
            }

            let value = `${files[el.newPosition.file]}${el.newPosition.rank}`;

            map[key].push(value);
        });

        this.setState({legalMoves: map})
    }

    onMouseOverSquare = square =>
    {
        if (this.state.pieceSquare != "") return; 

        var moves = this.getLegalMoves(square);

        var styles = this.highlightLegalMoves(moves);

        this.setState({squareStyles: styles});
    }

    onMouseOutSquare = square => 
    {
        if (this.state.pieceSquare == "")
        {
            this.setState({squareStyles: {}});
        }
    }

    onSquareRightClick = square =>
    {
        this.setState({squareStyles: {}, pieceSquare: ""});
    }

    getLegalMoves(square)
    {
        let moves = this.state.legalMoves[square];

        if (moves === undefined) return [];

        return moves;
    }

    highlightLegalMoves(moves)
    {
        let styles = {};
        moves.forEach(el => {
            styles[el] = {
                    background: "radial-gradient(circle, #fffc00 36%, transparent 40%)",
                    borderRadius: "50%"
            };
        });

        return styles;
    }

    onSquareClick = square => 
    {
        var moves = this.getLegalMoves(square);

        // square clicked already, meaning this is a moving move
        if (this.state.pieceSquare != "")
        {
            this.makeMove(this.state.pieceSquare, square);
            return;
        }

        var styles = this.highlightLegalMoves(moves);

        styles[square] = {
            backgroundColor:"pink"
        }

        this.setState({squareStyles: styles, pieceSquare: square}); 
    }

    makeMove = (from, to) =>
    {
        var legalMoves = this.getLegalMoves(from);

        if (!legalMoves.includes(to))
        {
            this.setState({pieceSquare: "", squareStyles: {}})
        }
    }

    render()
    {
        return (
            <div>
                <h3>CHESS</h3>
                <Chessboard 
                    onMouseOverSquare = {this.onMouseOverSquare}
                    onMouseOutSquare = {this.onMouseOutSquare}
                    onSquareClick = {this.onSquareClick}
                    onSquareRightClick = {this.onSquareRightClick}
                    squareStyles = {this.state.squareStyles}
                    position = {this.state.fen}
                />

                <button onClick={() => console.log(this.state)}>log state</button>
            </div>
            )
    }
}