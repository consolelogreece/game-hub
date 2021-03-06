﻿import React, { Component } from 'react';
import Tile from './Tile';

export default class Board extends Component {
    constructor(props) {
        super(props);
        this.state = {
            render: false,
            boardState: props.boardState,
            tileWidth: 0,
            borderThickness: 0
        };
    }

    static getDerivedStateFromProps(props, state)
    {
        var borderThickness = props.containerWidth / 75;

        // in order for the board to appear properly, the tile width must take in to consideration the border thickness.
        // if it doesnt, the containerWidth prop wont be adjusted properly, meaning tiles dont all fit into the row.
        // must times borderThickness by 2, to factor in BOTH sides
        var tileWidth = Math.floor(props.containerWidth / props.boardState[0].length) - (borderThickness * 2 / props.boardState[0].length);

        if (borderThickness !== state.borderThickness || tileWidth !== state.tileWidth)
        {
            return {
                boardState: props.boardState, 
                tileWidth: tileWidth,
                borderThickness: borderThickness
            }
        }  

        return null;
    }
    
    getDefaultBoard = () =>
    {
        let board = [];

        for (let i = 0; i < 6; i++) {
            let row = [];
            for (let j = 0; j < 7; j++) {
                row.push("white");
            }
            board.push(row);
        }

        return board
    }

    generateBoard = (boardState) => 
    {
        if (boardState === null) return this.getDefaultBoard();

        let boardRender = [];

        for (let i = boardState.length - 1; i >= 0; i--) {
            let row = [];
            for (let j = 0; j < boardState[i].length; j++) {
                row.push(<Tile 
                    className="tile"
                    color={boardState[i][j]}
                    boardColor={this.props.boardColor} 
                    move={(col) => this.props.move(col)} 
                    column={j}
                    tileDiameter = {this.state.tileWidth}
                    key={i + "-" + j}
                    />)
            }

            boardRender.push(
                <div key={i} className="row">
                    {row}
                </div>
            )
        }

        return boardRender;
    }

    render = () => {
        let boardRender = this.generateBoard(this.state.boardState);

        return (
            <div className="board" style={{
                backgroundColor: this.props.boardColor, 
                border:`${this.state.borderThickness}px solid ${this.props.boardColor}`, 
                borderRadius: "10px"
            }}>
                {boardRender}
            </div>
        )
    }
}