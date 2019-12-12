import React from 'react';
import Button from '../Button';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTimes } from '@fortawesome/free-solid-svg-icons'
import './styles.scss'

export default props => 
{
    let popupSuperContainerClasses = props.superContainerClassNames == undefined ? "popup-super-container" : props.superContainerClassNames;
    return (
        <div className={popupSuperContainerClasses} style={{...props.superContainerStyles}}>
            <div className={"popup-container " + props.containerClassNames} onClick={props.onClose !== undefined ? props.onClose : () => {}}>
                <div onClick={e => {e.stopPropagation()}} style={props.popupStyles} className="popup-content">
                    {props.onClose !== undefined && (<Button classNames="popup-close-button" onClick={props.onClose}><FontAwesomeIcon icon={faTimes} /></Button>)}
                    {props.children}
                </div>
            </div>
        </div>
    )
}