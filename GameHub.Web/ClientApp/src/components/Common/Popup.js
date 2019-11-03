import React from 'react';

export default props => 
{
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
                 left: "10%",
                 right: "10%",
                 top: "25%",
                 bottom: "25%",
                 margin: "auto",
                 background: "#f0d9b5",
                 borderRadius: "2%"
            }}>
                {props.children}
            </div>
        </div>
    )
}