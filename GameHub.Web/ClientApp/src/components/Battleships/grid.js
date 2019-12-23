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
        for (let i = 0; i < this.props.cols; i++)
        {
            for (let j = 0; j < this.props.rows; j++)
            {
                let style = this.getStyle(j, i);
                squares.push(
                    <div className="square" style={style}>
                        {j},{i}
                    </div>
                );
            }
        }

        return(
            <div ref={this.props.gridRef} {...this.props} id="grid">
                {squares}
                {this.props.children}
            </div>
        );
    }
}