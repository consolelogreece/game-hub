import React from 'react';
import './styles.scss';

let loading = "circle-loader";

export default props => {
    let renderSpecificClassContainer = loading;
    let renderSpecificIcon = <div />

    if(props.status === "success")
    {
            renderSpecificClassContainer = loading + " load-complete";
            renderSpecificIcon = (
                <div className={"checkmark draw"}></div>
            );
    }
    return (
        <div className={renderSpecificClassContainer}>
            {renderSpecificIcon}
        </div>
    )
}