import React, { Component } from 'react';
import withPopupOverlay from '../../../../HigherOrder/withTimedMessagePopup';
import Popup from '../../../../Popup';
import AbsoluteCenterAlign from '../../../../Common/AbsoluteCenter';
import './styles.scss';

let showClassNames = "battleships-board-notification-super-container-show";
let hideClassNames = "battleships-board-notification-super-container-hide";
let classNames = "battleships-board-notification-super-container";

export default function(WrappedComponent)
{
    class TimedErrorDropdown extends Component
    {
        render = () =>
        {
            let renderSpecificClass = this.props.shouldShowMessage ? showClassNames : hideClassNames;

            return (
                <div onClick={this.props.closeMessage}>
                    <Popup
                        containerClassNames={"battleships-board-popup-container"}
                        superContainerClassNames={[classNames, renderSpecificClass].join(" ")}
                    >   
                        <span onClick={this.props.closeMessage}>
                            {this.props.notificationMessage}
                        </span>
                    </Popup>
                </div>
            );
        };
    }

    return withPopupOverlay(WrappedComponent, TimedErrorDropdown, "displayBoardNotification", "notificationMessage", "closeMessage", "shouldShowMessage", 1200000);
}