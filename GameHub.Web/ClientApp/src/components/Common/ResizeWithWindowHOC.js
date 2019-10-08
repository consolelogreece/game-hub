import React, { Component } from 'react';

export default function(WrappedComponent)
{
    return class ResizeWithWindow extends Component
    {
        constructor(props)
        {
            super(props);
    
            this.state = {
                width: 0
            }

            this.wCom = React.createRef();
        }
    
        componentDidMount = () =>
        {        
            window.addEventListener('resize', this.updateDimensions);
        }
    
        componentWillUnmount = () =>
        {
            window.removeEventListener('resize', this.updateDimensions);
        }
    
        updateDimensions = event =>
        {
            var width = event == null ? 0 : event.srcElement.innerWidth;
            this.setState({width});
        }
    
        render = () =>
        {
            return (<WrappedComponent {...this.props} windowWidth={this.state.width} />);
        }
    }
}