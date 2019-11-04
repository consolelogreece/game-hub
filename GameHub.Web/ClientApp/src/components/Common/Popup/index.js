import React from 'react';
import './styles.css'

export default props => 
{
    return (
        <div className="popup-container" onClick={props.onClose !== undefined ? props.onClose : () => {}}>
            <div onClick={e => {e.stopPropagation()}} style={props.style} className="popup-content">
                {props.children}
            </div>
        </div>
    )
}