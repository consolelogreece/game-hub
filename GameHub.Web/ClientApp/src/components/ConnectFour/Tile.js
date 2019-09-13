import React, {Component} from 'react';

export default props => {
    return (
        <div onClick={() => props.makeMove(props.column)} style={{
            backgroundColor: props.color,
            borderRadius: "50%",
            width: "50px",
            height: "50px",
            display: "inline-block"
        }} />
    );
}