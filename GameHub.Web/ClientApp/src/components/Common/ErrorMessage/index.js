import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTimes } from '@fortawesome/free-solid-svg-icons'

import './styles.css'

export default props => 
{
    return (
        <div className="error-message-container">
            <div className="error-message-icon-container">
                <FontAwesomeIcon icon={faTimes} />
            </div>
            {props.text}
        </div>
    )
}