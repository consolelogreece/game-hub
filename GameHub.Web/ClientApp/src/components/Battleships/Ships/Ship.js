import React, {Component} from 'react';
export default class Ship extends Component
{
    render()
    {
        let body = [];

        let style = typeof(this.props.style) === "object" ? this.props.style : {};

        style.top = this.props.row * this.props.nPixelsSquare;
        style.left = this.props.col * this.props.nPixelsSquare;

        let Head = this.props.Head;

        let Body = this.props.Body;

        let {shipSegmentStyles} = this.props;

        for(let i = 1; i < this.props.length - 1; i++)
        {
            body.push(<Body style={shipSegmentStyles[i]} height={this.props.nPixelsSquare} key={i} width={this.props.nPixelsSquare} />);
        }

        return (
            <div style={style} name="ship" id={this.props.id} className={"ship-" + this.props.orientation}>
                {this.props.children}
                <Head style={shipSegmentStyles[0]} height={this.props.nPixelsSquare} width={this.props.nPixelsSquare} />
                {body}
                <Head style={shipSegmentStyles[shipSegmentStyles.length - 1]} height={this.props.nPixelsSquare} width={this.props.nPixelsSquare} />
            </div>
        );
    }
}