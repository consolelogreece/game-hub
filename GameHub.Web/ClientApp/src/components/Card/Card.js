import React from 'react';

export default function(props)
{
    return(
        <div>
            <div style={{position:"relative", height:"300px", width:"100%", overflow:"hidden"}}>
                <img style={{position:"absolute", bottom: 0, width:"100%"}} src={props.thumbnail}/>
            </div>
            <a href={props.url}>{props.name}</a>
        </div>
    );
}