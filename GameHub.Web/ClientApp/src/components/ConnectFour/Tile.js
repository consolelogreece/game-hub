import React, {Component} from 'react';

export default props => {
    return (
        <div 
            className="tile"
            onClick={() => props.makeMove(props.column)} style={{
            backgroundColor: props.color,
            borderRadius: "50%",
            width: props.tileDiameter + "px",
            height: props.tileDiameter + "px",
            border: `4px solid ${props.boardColor}`,
            boxSizing: "border-box"
        }} />
    );
}