import React, { Component } from 'react';
import withPopupOverlay from '../../../../HigherOrder/withTimedMessagePopup';
import Popup from '../../../../Popup';
import { transition_period } from'./styles.scss';

let showClassNames = "battleships-board-notification-super-container-show";
let hideClassNames = "battleships-board-notification-super-container-hide";
let classNames = "battleships-board-notification-super-container";

let showMessage = "battleships-board-notification-message-animate";
let messageClassName = "battleships-board-notification-message";

export default function(WrappedComponent)
{
    class TimedErrorDropdown extends Component
    {
        render = () =>
        {
            let renderSpecificClassContainer = this.props.shouldShowMessage ? showClassNames : hideClassNames;
            let renderSpecificClassMessage = (this.props.notificationTransitionState !== "none" || this.props.shouldShowMessage)  ? showMessage : "";

            return (
                <div>
                    <Popup
                        containerClassNames={"battleships-board-popup-container"}
                        superContainerClassNames={[classNames, renderSpecificClassContainer].join(" ")}
                    >   
                    </Popup>
                        <span className={[messageClassName, renderSpecificClassMessage].join(" ")}>
                            {this.props.notificationMessage}
                        </span>
                </div>
            );
        };
    }

    return withPopupOverlay(WrappedComponent, TimedErrorDropdown, "displayBoardNotification", "notificationMessage", "closeMessage", "shouldShowMessage","notificationTransitionState", transition_period , transition_period * 3);
}