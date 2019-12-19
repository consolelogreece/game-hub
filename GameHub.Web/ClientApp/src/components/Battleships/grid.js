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
        let squares = [];
        for (let i = 0; i < this.props.rows; i++)
        {
            for (let j = 0; j < this.props.cols; j++)
            {
                let style = this.getStyle(i,j);
                squares.push(
                    <div className="square" style={style}>
                        {i},{j}
                    </div>
                );
            }
        }

        return(
            <div id="grid">
                {squares}
                {this.props.children}
            </div>
        );
    }
}