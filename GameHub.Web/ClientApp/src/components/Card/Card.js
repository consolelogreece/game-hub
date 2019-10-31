import React from 'react';

export default function(props)
{
    return(
        <div style={{borderRadius:"5%", boxShadow: "5px 10px 18px #888888", padding:"10px"}}>
            <div style={{position:"relative", maxHeight:"300px", height:"20vw", width:"100%", overflow:"hidden"}}>
                <img style={{position:"absolute", bottom: 0, width:"100%"}} src={props.thumbnail}/>
            </div>
            <a href={props.url}>{props.name}</a>
        </div>
    );
}