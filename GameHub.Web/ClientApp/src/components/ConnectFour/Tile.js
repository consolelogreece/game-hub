import React from 'react';

export default props => {
    return (
        <div style={{
            backgroundColor: props.color,
            borderRadius: "50%",
            width: "50px",
            height: "50px",
            display: "inline-block"
        }} />
    );
}