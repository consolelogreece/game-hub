import React from 'react';
import '../../styles.scss';

export default props => 
{   
    return <div style={{height: props.height, width: props.width, ...props.style}} className="ship-head-inplay"/> 
}