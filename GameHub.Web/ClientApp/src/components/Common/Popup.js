import React from 'react';

export default props => 
{
    var x = props.items.map(item => {
        return (
            <button onClick={() => props.callback(item)}>
                {item}
            </button>
        )
    });

    return (
        <div style={{
                position: "fixed",
                width: "100%",
                height: "100%",
                top: "0",
                left: "0",
                right: "0",
                bottom: "0",
                margin: "auto",
                zIndex: "99",
                backgroundColor: "rgba(0,0,0, 0.5)"
            }}>
            <div style={{
                 position: "absolute",
                 left: "25%",
                 right: "25%",
                 top: "25%",
                 bottom: "25%",
                 margin: "auto",
                 background: "white"
            }}>
                <h1>{props.title}</h1>
                {x}
            </div>
        </div>
    )
}