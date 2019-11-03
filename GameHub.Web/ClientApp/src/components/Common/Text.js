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
            <h6 style={{font: 'bold 15px/24px "Haas Grot Text R Web", "Helvetica Neue", Helvetica, Arial, sans-serif'}}>
                {props.children}
            </h6>
        </div>
    )
}

export function TextArea(props)
{
    return(
        <div>
            <p style={{textAlign:"center"}}>
                {props.children}
            </p>
        </div>
    )
}
