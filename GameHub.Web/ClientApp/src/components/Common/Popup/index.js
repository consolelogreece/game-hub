import React from 'react';
import './styles.css'

export default props => 
{
    return (
        <div className={"popup-super-container" + " " + props.superContainerClassNames} style={{...props.superContainerStyles}}>
            <div className={"popup-container"  + " " + props.containerClassNames} onClick={props.onClose !== undefined ? props.onClose : () => {}}>
                <div onClick={e => {e.stopPropagation()}} style={props.popupStyles} className="popup-content">
                    {props.children}
                </div>
            </div>
        </div>
    )
}