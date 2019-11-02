import React from 'react'
import ErrorMessage from '../../ErrorMessage';
import './styles.css'
import { faUnderline } from '@fortawesome/free-solid-svg-icons';

// name of error related classes in styles.css
const errorStyle = "form-region-error";
const noErrorStyle = "form-region-no-error";

export default class FormRegion extends React.PureComponent {

    render()
    {
        let hasErrors = !(this.props.errors === undefined || this.props.errors.length === 0);
        let errorDependentClass = hasErrors ? errorStyle : noErrorStyle;

        return (
            <div className={`form-region ${errorDependentClass}`}>
                <div className="form-region-title-container"> 
                    <h6 className="form-region-title">{this.props.label}</h6>
                </div>
                {this.props.children}
                {hasErrors && (
                    <div className="form-region-error-message">
                        <ErrorMessage text={this.props.errors} />
                    </div>
                )}
            </div>
        )
    }
}