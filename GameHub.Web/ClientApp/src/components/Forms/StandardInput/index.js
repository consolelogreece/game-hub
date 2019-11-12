import React from 'react';
import './styles.scss';

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