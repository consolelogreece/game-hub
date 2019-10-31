import React from 'react';
import './styles.css'

export default class withHoverFadeOverlay extends React.Component
{
    constructor(props)
    {
        super(props);
    }
    render = () =>
    {
        return (
            <div className="fade_overlay_container">
                <div className="fade_overlay_element">
                    {this.props.children}
                </div>
                <div className="fade_overlay" style={{backgroundColor: this.props.fadeColor}}>
                    <div className="fade_overlay_text">{this.props.text}</div>
                </div>
            </div>
        )
    }
}