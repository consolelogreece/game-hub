import React, { Component } from 'react';
import { timeout } from '../../../utils/sleep';
import Popup from '../../Popup';
import { transition_period } from './styles.scss';

let show = "timed-error-super-container-show";
let hide = "timed-error-super-container-hide";

export default function(WrappedComponent, duration)
{
    return class TimedErrorDropdown extends Component
    {
        constructor(props)
        {
            super(props);

            this.state = {
                message: "",
                messageTimeoutDurationMS: duration === undefined ? 3000 : duration,
                render: false
            };
        }

        display = async message =>
        {
            // don't bother changing if same message as can cause rendering issues.
            if (this.state.message === message) return;

            if (message === undefined) return;
        
            if(this.state.message !== "")
            {
                await this.close();
            }

            this.setState({message:message, render: true}, async () =>
            {
                await timeout(this.state.messageTimeoutDurationMS);

                // if the message has changed, dont wipe it as it's the responsibilty of another process now. wiping it can cause shortened messages.
                if (this.state.message === message)
                {
                    this.close();
                }
            });
        }

        close = async () =>
        {
            this.setState({render: false});

            await timeout(transition_period);

            this.setState({message: ""});
        }

        render = () =>
        {
            let renderSpecificClass = this.state.render ? show : hide;
            return (
                <div>
                    <div onClick={this.close}>
                        {this.state.message !== "" && (
                            <Popup
                                superContainerClassNames={"timed-error-super-container " + renderSpecificClass}
                            >
                                <span onClick={this.close}>
                                    {this.state.message}
                                </span>
                            </Popup>
                        )}
                     </div>
                    <WrappedComponent 
                        {...this.props}
                        displayTimedError={this.display}
                    />
                </div>
            );
        }
    }
}