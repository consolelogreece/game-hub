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
            // square styles for active drop square
            dropSquareStyle: {},
            // custom square styles
            squareStyles: {},
            // square with the currently clicked piece
            pieceSquare: "",
            // currently clicked square
            square: "",
            // array of past game moves
            history: []
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
        console.log("moves", moves);
    }

    render()
    {
        return (
            <div>
                <h3>CHESS</h3>
                <Chessboard position={this.state.fen}/>
            </div>
            )
    }
}

const squareStyling = ({ pieceSquare, history }) => {
  const sourceSquare = history.length && history[history.length - 1].from;
  const targetSquare = history.length && history[history.length - 1].to;

  return {
    [pieceSquare]: { backgroundColor: "rgba(255, 255, 0, 0.4)" },
    ...(history.length && {
      [sourceSquare]: {
        backgroundColor: "rgba(255, 255, 0, 0.4)"
      }
    }),
    ...(history.length && {
      [targetSquare]: {
        backgroundColor: "rgba(255, 255, 0, 0.4)"
      }
    })
  };
};
