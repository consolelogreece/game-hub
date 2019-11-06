import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChessBishop, faChessRook, faChessQueen, faChessKnight } from '@fortawesome/free-solid-svg-icons'
import './Chess.css'

export default props =>
{
    return (
        <div style={{display: "flex", flexDirection: "column", justifyContent: "center", height:"90%", margin:"5px"}}>
            <div style={{display: "flex", flexDirection: "row", height: "100%"}}>
                <button className="button-promotion" onClick={() => props.callback("Q")}>
                    <FontAwesomeIcon icon={faChessQueen} />
                </button>
                <button className="button-promotion" onClick={() => props.callback("R")}>
                    <FontAwesomeIcon icon={faChessRook} />
                </button>
            </div>
            <div style={{display: "flex", flexDirection: "row", height: "100%"}}>
                <button className="button-promotion" onClick={() => props.callback("B")}> 
                    <FontAwesomeIcon icon={faChessBishop} />
                </button>
                <button className="button-promotion" onClick={() => props.callback("N")}>
                    <FontAwesomeIcon icon={faChessKnight} />
                </button>
            </div>

        </div>
    );
}

