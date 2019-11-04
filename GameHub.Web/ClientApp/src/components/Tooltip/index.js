import React from 'react';
import './styles.css';

export default props => (
    
    <div class="fancy-tooltip">{props.pretext}
        <span class="fancy-tooltip-text">{props.tooltip}</span>
    </div>
);