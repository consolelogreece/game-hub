import React, {Component} from 'react';

export default class Ship extends Component
{   
    constructor(props)
    {
        super(props);
    }

    render()
    {
        let body = [];

        let style = this.props.style;

        style.top = this.props.y * this.props.nPixelsSquare;

        style.left = this.props.x * this.props.nPixelsSquare;

        for(let i = 1; i < this.props.length - 1; i++)
        {
            body.push(<div className="ship-body"/>);
        }

        return (
            <div style={style} name="ship" id={this.props.id} className={"ship-" + this.props.orientation}>
                {this.props.children}
                <div className="ship-head"/>
                {body}
                <div className="ship-head"/>
            </div>
        );
    }
}