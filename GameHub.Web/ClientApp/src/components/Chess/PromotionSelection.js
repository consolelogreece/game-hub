import React from 'react';

import './Chess.css'

export default props =>
{
    return (
        <div style={{display: "flex", flexDirection: "column", justifyContent: "center", height:"90%", maxWidth: "400px"}}>
            <div style={{display: "flex", flexDirection: "row", height: "100%"}}>
                <button className="button-promotion" onClick={() => props.callback("Q")}>
                    ♕
                </button>
                <button className="button-promotion" onClick={() => props.callback("R")}>
                    ♖
                </button>
            </div>
            <div style={{display: "flex", flexDirection: "row", height: "100%"}}>
                <button className="button-promotion" onClick={() => props.callback("B")}> 
                    ♗
                </button>
                <button className="button-promotion" onClick={() => props.callback("N")}>
                    ♘
                </button>
            </div>

        </div>
    );
}

