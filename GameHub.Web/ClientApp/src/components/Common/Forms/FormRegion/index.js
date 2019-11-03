import React from 'react';
import ErrorMessage from '../../ErrorMessage';
import './styles.css'
import { faUnderline } from '@fortawesome/free-solid-svg-icons';

// name of error related classes in styles.css
const errorStyleInput = "form-region-error";
const noErrorStyleInput = "form-region-no-error";
const errorStyleMessage = "form-region-error-message-show";
const noErrorStyleMessage = "form-region-error-message-hide";

export default class FormRegion extends React.PureComponent {

    constructor(props)
    {
        super(props)
        {
            this.state = {
                errorMessage: "",
                messageRenderClassName:errorStyleMessage,
                inputRenderClassName:""
            };
        }
    }

    componentDidUpdate(prevProps, _, __)
    {
        if (prevProps.errors !== undefined && prevProps.errors.length !== 0)
        {
            if (this.props.errors === undefined || this.props.errors.length === 0)
            {
                this.setState({messageRenderClassName: noErrorStyleMessage}, () => {
                    setTimeout(() => {
                        this.setState({
                            errorMessage: this.props.errors,
                            messageRenderClassName: errorStyleMessage
                        })
                    }, 300)
                });
            }
        }
        else
        {
            if (this.state.messageRenderClassName !== noErrorStyleMessage)
            {
                this.setState({
                    errorMessage: this.props.errors
                });
            }
        }
    }

    render()
    {
        let hasErrors = !(this.state.errorMessage === undefined || this.state.errorMessage.length === 0);
        let errorDependentClass = hasErrors ? errorStyleInput : noErrorStyleInput;

        return (
            <div className={`form-region ${errorDependentClass}`}>
                <div className="form-region-title-container"> 
                    <label className="form-region-label">{this.props.label}</label>
                </div>
                {this.props.children}
                {hasErrors && (
                    <div className={this.state.messageRenderClassName}>
                        <div className="form-region-error-message">
                            <ErrorMessage text={this.state.errorMessage} />
                        </div>
                    </div>
                )}
            </div>
        )
    }
}