import React from 'react';
import { noAuto } from '@fortawesome/fontawesome-svg-core';

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
                backgroundColor: "rgba(0,0,0, 0.5)",
                textAlign: "center"

            }} onClick={props.onClose !== undefined ? props.onClose : () => {}}>
            <div style={{
                 width: "auto",
                 position:"relative",
                 top: "50%",
                 transform: "translateY(-50%)",
                 margin: "0 auto",
                 display: "inline-block",
                 background: "#f0d9b5",
                 borderRadius: "2%"
            }}>
                {props.children}
            </div>
        </div>
    )
}