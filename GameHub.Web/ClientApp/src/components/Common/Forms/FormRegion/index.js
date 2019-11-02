import React from 'react'
import ErrorMessage from '../../ErrorMessage';
import './styles.css'

// name of error related classes in styles.css
const errorStyle = "form-region-error";
const noErrorStyle = "form-region-no-error";

export default props => {
    const hasErrors = !(props.error === undefined || props.error.message.length === 0);
    const errorDependentClass = hasErrors ? errorStyle : noErrorStyle;
  
    let bitchname = props.error.shouldRender ? "form-error-transition" : "form-no-error-transition";

    if (hasErrors)
    {
      
        console.log(bitchname)

    }


    return (
        <div className={`form-region ${errorDependentClass}`}>
            <h6>{props.label}</h6>
            {props.children}

            {hasErrors && (
                <div className={bitchname}>
                    <ErrorMessage text={props.error.message + props.error.message + props.error.message } />
                </div>
            )}
        </div>
    )
}