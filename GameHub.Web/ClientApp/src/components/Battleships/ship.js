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

        for(let i = 1; i < this.props.length - 1; i++)
        {
            body.push(<div className="ship-body"/>);
        }

        return (
            <div id={this.props.id} draggable={true} className={"ship-" + this.props.orientation}>
                <div className="ship-head"/>
                {body}
                <div className="ship-head"/>
            </div>
        );
    }
}

// onDrag={this.props.handleDrag}