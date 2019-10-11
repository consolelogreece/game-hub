import React from 'react';

export function Title(props)
{
    return(
        <div>
            <h4 style={{textAlign:"center"}}>
                {props.text}
            </h4>
        </div>
    )
}

export function Subtitle(props)
{
    return(
        <div>
            <h6 style={{textAlign:"center"}}>
                {props.text}
            </h6>
        </div>
    )
}
