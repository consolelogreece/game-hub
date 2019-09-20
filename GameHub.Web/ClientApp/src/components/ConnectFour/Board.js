﻿import React, { Component } from 'react';
import Tile from './Tile';


///TODO NEXT: WHY DO I HAVE TO RESIZE TO GET FIRST RENDER? UPDATED PROPS ISNT INITIATING RERENDER FOR SOME REASON FUCK
export default class Board extends Component {
    constructor(props) {
        super(props);
        this.state = {
            render: false,
            tileWidth: 0
        };
 
        this.updateDimensions = this.updateDimensions.bind(this);
    }

    componentDidMount()
    {           
        window.addEventListener('resize', this.updateDimensions);
        this.updateDimensions();
    }

    componentWillUnmount()
    {
        window.removeEventListener('resize', this.updateDimensions);
    }

    componentWillReceiveProps(props)
    {
        this.updateDimensions()
        this.setState({render: !this.state.render})
    }

    updateDimensions() 
    {

        console.log(this.props, this.state)
        var containerWidth = this.refs.board.clientWidth;

        var tileWidth = Math.floor(containerWidth / this.props.boardState[0].length)

        this.setState({tileWidth: tileWidth});

        
    }

    render = () => {
        let boardRender = [];

        for (let i = this.props.boardState.length - 1; i >= 0; i--) {
            let row = [];
            for (let j = 0; j < this.props.boardState[i].length; j++) {
                row.push(<Tile 
                    className="tile"
                    color={this.props.boardState[i][j]}
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
            <div ref="board" className="board" style={{backgroundColor: this.props.boardColor, border: `4px solid ${this.props.boardColor}`, borderRadius: "5%"}}>
                {boardRender}
                <button onClick={() => console.log(this.props)}>log board</button>
            </div>
        )
    }
}