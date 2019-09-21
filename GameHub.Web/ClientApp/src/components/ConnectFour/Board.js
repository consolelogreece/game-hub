import React, { Component } from 'react';
import Tile from './Tile';

export default class Board extends Component {
    constructor(props) {
        super(props);
        this.state = {
            render: false,
            boardState: props.boardState,
            tileWidth: 0
        };

        this.count = 0;
 
        this.updateDimensions = this.updateDimensions.bind(this);
    }

    componentWillReceiveProps(props)
    {
        this.setState({boardState: props.boardState}, () => this.updateDimensions());    
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

    updateDimensions() 
    {
        var containerWidth = this.refs.board.clientWidth;

        var tileWidth = Math.floor(containerWidth / this.state.boardState[0].length)

        this.setState({tileWidth: tileWidth});
    }

    render = () => {
        let boardRender = [];

        console.log("rendering", this.state)
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
            <div ref="board" className="board" style={{backgroundColor: this.props.boardColor, border: `4px solid ${this.props.boardColor}`, borderRadius: "5%"}}>
                {boardRender}
            </div>
        )
    }
}