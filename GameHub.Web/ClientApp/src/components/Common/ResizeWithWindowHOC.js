import React, { Component } from 'react';

export default function(component)
{
    return class ResizeWithWindow extends Component
    {
        constructor(props)
        {
            super(props);
    
            this.state = {
                width: 0
            }
        }
    
        componentDidMount = () =>
        {           
            window.addEventListener('resize', this.updateDimensions);
            //this.updateDimensions();
        }
    
        componentWillUnmount = () =>
        {
            window.removeEventListener('resize', this.updateDimensions);
        }
    
        updateDimensions = event =>
        {
            console.log("\nevent: " + event.originalTarget.innerWidth + "\nref: " + this.refs.component.clientWidth)
        }
    
        render()
        {
            return <component ref="component" {...this.props} windowWidth={this.state.width} />
        }
    }
}