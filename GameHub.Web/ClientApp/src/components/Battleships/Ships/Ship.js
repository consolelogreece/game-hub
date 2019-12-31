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

        let style = typeof(this.props.style) === "object" ? this.props.style : {};

        style.left = this.props.row * this.props.nPixelsSquare;

        style.top = this.props.col * this.props.nPixelsSquare;

        let Head = this.props.Head;

        let Body = this.props.Body;

        for(let i = 1; i < this.props.length - 1; i++)
        {
            body.push(<Body height={this.props.nPixelsSquare} width={this.props.nPixelsSquare} />);
        }

        return (
            <div style={style} name="ship" id={this.props.id} className={"ship-" + this.props.orientation}>
                {this.props.children}
                <Head height={this.props.nPixelsSquare} width={this.props.nPixelsSquare} />
                {body}
                <Head height={this.props.nPixelsSquare} width={this.props.nPixelsSquare} />
            </div>
        );
    }
}