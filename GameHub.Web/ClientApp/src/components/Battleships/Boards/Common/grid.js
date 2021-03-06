import React, {Component} from 'react';
import { gridSquareBlue } from '../../../../utils/variables';

import '../../styles.scss';

let defaultStyle = {
  backgroundColor: gridSquareBlue
};

export default class BattleshipsGrid extends Component 
{
    getStyle = (row, col) =>
    {
        let key = `${row},${col}`;

        if (this.props.styles !== undefined && this.props.styles.hasOwnProperty(key)) 
        {
            return this.props.styles[key];
        }

        return defaultStyle;
    }

    render()
    {
        let squareSize = this.props.nPixelsSquare === undefined ? 40 : this.props.nPixelsSquare;

        let squares = [];
        for (let row = 0; row < this.props.cols; row++)
        {
            for (let col = 0; col < this.props.rows; col++)
            {
                let style = {...this.getStyle(row, col)};
                style.height = squareSize;
                style.width = squareSize;
                
                squares.push(
                    <div onClick={() => this.props.onSquareClick(row, col)} className="square" key={`${row},${col}`} style={style} />
                );
            }
        }

        // remove props we dont want on dom tree
        let {nPixelsSquare, onSquareClick, gridRef, ...props} = this.props;

        return(
            <div style={{height: props.height, width: props.width}} ref={gridRef} {...props} id="grid">
                {squares}
                {this.props.children}
            </div>
        );
    }
}