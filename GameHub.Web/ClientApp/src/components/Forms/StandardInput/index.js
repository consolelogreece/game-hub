import React from 'react';
import './styles.css';

export default class StandardInput extends React.PureComponent 
{
    render() {
        return (
            <div className="standard-container">
                <input 
                    className="standard-input" 
                    name={this.props.name}
                    value={this.props.value}
                    onChange={this.props.onValueChange}
                />
            </div>
        )
    }
}