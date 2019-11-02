import React from 'react'
import ErrorMessage from '../../ErrorMessage';
import './styles.css'
import { faUnderline } from '@fortawesome/free-solid-svg-icons';

// name of error related classes in styles.css
const errorStyle = "form-region-error";
const noErrorStyle = "form-region-no-error";

export default props => {
    const hasErrors = !(props.errors === undefined || props.errors.length === 0);
    const errorDependentClass = hasErrors ? errorStyle : noErrorStyle;

    return (
        <div className={`form-region ${errorDependentClass}`}>
            <h6>{props.label}</h6>
            {props.children}

            {hasErrors && (
                <div className="form-region-error-message">
                    <ErrorMessage text={props.errors} />
                </div>
            )}
        </div>
    )
}