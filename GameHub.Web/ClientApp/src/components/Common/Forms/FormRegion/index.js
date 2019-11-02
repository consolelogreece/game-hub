import React from 'react'
import ErrorMessage from '../../ErrorMessage';
import './styles.css'

// name of error class in styles.css
const errorStyle = "form-region-error";

export default props => {
    const hasErrors = !!props.errors;
    const errorDependentClass = hasErrors ? errorStyle : "";

    console.log(errorDependentClass)
    return (
        <div className={`form-region form-region-error`}>
            <h6>{props.label}</h6>
            {hasErrors && <ErrorMessage text={props.errors} />}
            {props.children}
        </div>
    )
}