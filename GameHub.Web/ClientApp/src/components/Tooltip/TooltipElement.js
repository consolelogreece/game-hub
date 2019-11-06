import React from 'react';
import './styles.css'

export default class TooltipElement extends React.PureComponent
{
    componentDidMount()
    {
        // Manually update popper.
        // I needed to do this as the popper doesn't know about the animation so it doesn't update the position when it's complete,
        // Meaning the location of the tooltip is wrong.
        setTimeout(() => {this.props.scheduleUpdate()}, this.props.transitionPeriod);
    }
    render()
    {
        return (
            <div ref={this.props.annahroof} id="tooltip-pop-element-body" className={this.props.renderSpecificClass} style={{...this.props.style, }} data-placement={this.props.placement}>
                {this.props.tooltip}
                <div ref={this.props.arrowProps.ref} id="tooltip-pop-element-arrow" className={this.props.renderSpecificClass} style={{...this.props.arrowProps.style}} />
            </div>
        );
    }
}