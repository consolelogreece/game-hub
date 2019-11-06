import React from 'react';
import Button from '../../Button';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTimes } from '@fortawesome/free-solid-svg-icons'
import './styles.css'

export default props => 
{
    return (
        <div className={"popup-super-container" + " " + props.superContainerClassNames} style={{...props.superContainerStyles}}>
            <div className={"popup-container"  + " " + props.containerClassNames} onClick={props.onClose !== undefined ? props.onClose : () => {}}>
                <div onClick={e => {e.stopPropagation()}} style={props.popupStyles} className="popup-content">
                    <Button classNames="popup-close-button" onClick={props.onClose}><FontAwesomeIcon icon={faTimes}/></Button>
                    {props.children}
                </div>
            </div>
        </div>
    )
}