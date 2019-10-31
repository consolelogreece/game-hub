import React from 'react';

export default props => {
    return (
        <div style={{display: "flex", flexWrap: "wrap"}}>
            {props.elements.map(g => (
                <div style={{width:"40%", margin: "0% 5%"}}>
                    {g}
                </div>
            ))}
        </div>
    )
}