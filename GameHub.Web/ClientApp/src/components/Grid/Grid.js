import React from 'react';

export default props => {
    let els = props.elements.map((G, i) => {
        return (
            <div key={i} style={{width:"40%", margin: "0% 5%"}}>
                {G}
            </div>
        )}
    )

    return (
        <div style={{display: "flex", flexWrap: "wrap"}}>
            {els}
        </div>
    )
}