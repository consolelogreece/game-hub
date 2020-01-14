import React, { Component } from 'react';
import withPopupOverlay from '../withTimedMessagePopup';
import Popup from '../../Popup';
import './styles.scss';

let showClassNames = "timed-error-super-container-show";
let hideClassNames = "timed-error-super-container-hide";
let classNames = "timed-error-super-container";

export default function(WrappedComponent, duration)
{
    class TimedErrorDropdown extends Component
    {
        render = () =>
        {
            let renderSpecificClass = this.props.shouldShowError ? showClassNames : hideClassNames;

            return (
                <div>
                    <div onClick={this.props.closeError}>
                        <Popup
                            superContainerClassNames={[classNames, renderSpecificClass].join(" ")}
                        >
                            <span onClick={this.props.closeError}>
                                {this.props.errorMessage}
                            </span>
                        </Popup>
                     </div>
                </div>
            );
        };
    }

    return withPopupOverlay(WrappedComponent, TimedErrorDropdown, "displayTimedError", "errorMessage", "closeError", "shouldShowError", duration);
}