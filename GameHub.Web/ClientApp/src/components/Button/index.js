import React from 'react'
import './styles.css';

export default class Button extends React.PureComponent
{
    render()
    {
        // default css class if none provided
        let btnClass = this.props.classNames === undefined ? "fancy-button" : this.props.classNames;
        console.log(this.props)
        return (
            <div style={this.props.style} className={btnClass} onClick={this.props.onClick}>
                {this.props.children}
            </div>
        )
    }
}