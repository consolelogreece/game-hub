import React from 'react';
import './styles.scss';

export default function AbsoluteCenterAlign(props)
{
    return (
        <div className="absolute-center-align">
            {props.children}
        </div>
    )
}