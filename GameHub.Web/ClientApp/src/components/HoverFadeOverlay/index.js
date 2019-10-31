import React from 'react';
import './styles.css'

export default props =>
{
    return (
        <div className="fade_overlay_container">
            <div className="fade_overlay_element">
                {props.children}
            </div>
            <div className="fade_overlay" style={{backgroundColor: props.fadeColor}}>
                <div className="fade_overlay_text">{props.text}</div>
            </div>
        </div>
        )
}