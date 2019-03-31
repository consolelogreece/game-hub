import React, { Component } from 'react';
import Tile from './Tile';

export default class Board extends Component {
    constructor(props) {
        super(props);
        this.state = {
            nRows: 6,
            nCols: 7,
            render: false
        };
    }



    render = () => {
        let boardRender = [];

        for (let i = this.props.boardState.length - 1; i >= 0; i--) {
            let row = [];
            for (let j = 0; j < this.props.boardState[i].length; j++) {
                row.push(<Tile color={this.props.boardState[i][j]} />)
            }

            boardRender.push(
                <div>
                    {row}
                </div>
            )
        }

        console.log(this.props.boardState);

        return (
            <div>
                {boardRender}
            </div>
        )
    }
}
