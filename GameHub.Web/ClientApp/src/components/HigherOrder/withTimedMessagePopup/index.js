import React, { Component } from 'react';
import { timeout } from '../../../utils/sleep';
import { transition_period } from './styles.scss';

export default function(WrappedComponent, PopupComponent,
    displayPropName = "display", 
    messagePropName = "message", 
    closePropName = "close", 
    showPropName = "show", 
    duration = 3000)
{
    return class WithTimedPopup extends Component
    {
        constructor(props)
        {
            super(props);

            this.state = {
                message: "",
                messageTimeoutDurationMS: duration,
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
            let PopupComponentProps = {};
            PopupComponentProps[closePropName] = this.close;
            PopupComponentProps[showPropName] = this.state.render;
            PopupComponentProps[messagePropName] = this.state.message;

            let WrappedComponentProps = {};
            WrappedComponentProps[displayPropName] = this.display;

            let Message = this.state.message !== "" ? <PopupComponent {...PopupComponentProps} /> : <div />;
            WrappedComponentProps[messagePropName] = Message;


            return (
                <React.Fragment>
                    <WrappedComponent 
                        {...this.props}
                        {...WrappedComponentProps}
                    />
                </React.Fragment>
            );
        }
    }
}