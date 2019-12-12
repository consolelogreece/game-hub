import React, { Component } from 'react';
import { timeout } from '../../../utils/sleep';
import Popup from '../../Popup';
import './styles.scss';

export default function(WrappedComponent, duration)
{
    return class TimedErrorDropdown extends Component
    {
        constructor(props)
        {
            super(props);

            this.state = {
                message: "",
                messageTimeoutDurationMS: duration == undefined ? 3000 : duration
            };
        }

        display = async message =>
        {
            // don't bother changing if same message as can cause rendering issues.
            if (this.state.message == message) return;

            if (message == undefined) return;

            this.setState({message:message}, async () =>
            {
                await timeout(this.state.messageTimeoutDurationMS);

                // if the message has changed, dont wipe it as it's the responsibilty of another process now. wiping it can cause shortened messages.
                if (this.state.message == message)
                {
                    this.setState({message: ""});
                }
            });
        }

        render = () =>
        {
            return (
                <div>
                    {this.state.message != "" && (
                        <Popup
                            superContainerClassNames={"timed-error-super-container"}
                        >
                            <span style={{color: "red"}}>
                                {this.state.message}
                            </span>
                        </Popup>
                    )}
                    <WrappedComponent 
                        {...this.props}
                        displayTimedError={this.display}
                    />
                </div>
            );
        }
    }
}