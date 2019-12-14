import React from 'react';
import './styles.scss';

let loading = "circle-loader";

export default props => {
    let renderSpecificClassContainer = loading;
    let renderSpecificIcon = <div />

    switch(props.status)
    {
        case "success":  
            renderSpecificClassContainer = loading + " load-complete-success";
            renderSpecificIcon = (
                <div className={"checkmark draw"}></div>
            );
            break;
        case "failure":
            renderSpecificClassContainer = loading + " load-complete-failure";
            renderSpecificIcon = (
               <div className={"cross-container"}>
                   <div className={"cross1 draw"}></div>
                   <div className={"cross2 draw"}></div>
               </div>
            )
            break;
    }

    return (
        <div className={renderSpecificClassContainer}>
            {renderSpecificIcon}
        </div>
    )
}