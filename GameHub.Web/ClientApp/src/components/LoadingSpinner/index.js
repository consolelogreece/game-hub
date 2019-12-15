import React from 'react';
import './styles.scss';

export default props => {
    let renderSpecificClassContainer = loading;
    let renderSpecificIcon = <div />

    let loading = "circle-loader";

    let colorModifier = props.colorScheme;

    loading += "-" + colorModifier;

    switch(props.status)
    {
        case "success":  
            renderSpecificClassContainer = loading + " load-complete-success-" + colorModifier;
            renderSpecificIcon = (
                <div className={"checkmark draw"}></div>
            );
            break;
        case "failure":
            renderSpecificClassContainer = loading + " load-complete-failure-" + colorModifier;
            renderSpecificIcon = (
               <div className={"cross-container-" + colorModifier}>
                   <div className={`cross1-${colorModifier} draw`}></div>
                   <div className={`cross2-${colorModifier} draw`}></div>
               </div>
            )
            break;
    }

    console.log(renderSpecificClassContainer);

    return (
        <div className={renderSpecificClassContainer}>
            {renderSpecificIcon}
        </div>
    )
}