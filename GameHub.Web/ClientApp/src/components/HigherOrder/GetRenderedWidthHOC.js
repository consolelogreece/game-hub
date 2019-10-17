import React, { Component } from 'react';

export default function(WrappedComponent)
{
    return class GetRenderedWidth extends Component
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
            this.updateDimensions();
        }
    
        componentWillUnmount = () =>
        {
            window.removeEventListener('resize', this.updateDimensions);
        }
    
        updateDimensions = () =>
        {
            this.setState({width: this.wCom.current.clientWidth});
        }
    
        render = () =>
        {
            return (
                <div style={{width: "100%"}} ref={this.wCom}>
                    <WrappedComponent {...this.props} containerWidth={this.state.width} />
                </div>
                );
        }
    }
}