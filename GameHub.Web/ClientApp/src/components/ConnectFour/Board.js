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

    componentWillReceiveProps(props)
    {
        var borderThickness = props.containerWidth / 20;

        // in order for the board to appear properly, the tile width must take in to consideration the border thickness. if it doesnt, the containerWidth prop wont be adjusted properly, meaning tiles dont all fit into the row.
        var tileWidth = Math.floor(props.containerWidth / props.boardState[0].length) - (borderThickness / props.boardState[0].length);

        this.setState({
            boardState: props.boardState, 
            tileWidth: tileWidth,
            borderThickness: borderThickness
        });    
    }

    render = () => {
        let boardRender = [];

        for (let i = this.state.boardState.length - 1; i >= 0; i--) {
            let row = [];
            for (let j = 0; j < this.state.boardState[i].length; j++) {
                row.push(<Tile 
                    className="tile"
                    color={this.state.boardState[i][j]}
                    boardColor={this.props.boardColor} 
                    makeMove={(col) => this.props.makeMove(col)} 
                    column={j}
                    tileDiameter = {this.state.tileWidth}
                    />)
            }

            boardRender.push(
                <div className="row">
                    {row}
                </div>
            )
        }

        return (
            <div className="board" style={{
                backgroundColor: this.props.boardColor, 
                border:`${this.state.borderThickness} solid ${this.props.boardColor}`, 
                borderRadius: "10px"
            }}>
                {boardRender}
            </div>
        )
    }
}