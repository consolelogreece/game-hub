import React, {Component} from 'react';
import './styles.scss';

let defaultStyle = {
  backgroundColor: "#333"
};

export default class BattleshipsGrid extends Component 
{
    getStyle = (row, col) =>
    {
        let key = `${row},${col}`;

        if (this.props.styles != undefined && this.props.styles.hasOwnProperty(key)) 
        {
            return this.props.styles[key];
        }

        return defaultStyle;
    }

    render()
    {
        let squareSize = this.props.nPixelsSquare === undefined ? 40 : this.props.nPixelsSquare;

        let squares = [];
        for (let i = 0; i < this.props.cols; i++)
        {
            for (let j = 0; j < this.props.rows; j++)
            {
                let style = {...this.getStyle(j, i)};
                style.height = squareSize;
                style.width = squareSize;
                squares.push(
                    <div className="square" style={style}>
                        {j},{i}
                    </div>
                );
            }
        }

        return(
            <div style={{height: this.props.height, width: this.props.width}} ref={this.props.gridRef} {...this.props} id="grid">
                {squares}
                {this.props.children}
            </div>
        );
    }
}