import React from 'react'
import './styles.css';

export default class Button extends React.PureComponent
{
    render()
    {
        return (
            <div style={this.props.style} className="fancy-button" onClick={this.props.onClick}>
                {this.props.children}
            </div>
        )
    }
}