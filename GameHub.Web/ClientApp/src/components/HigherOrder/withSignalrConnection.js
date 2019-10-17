import { HubConnectionBuilder } from '@aspnet/signalr';

import React, { Component } from 'react';

export default function(WrappedComponent, endpoint)
{
    return class InjectSignalrConnection extends Component
    {
        constructor(props)
        {
            super(props);
    
            this.state = {
                permanentInvokeParams:[],
                connection: new HubConnectionBuilder()
                .withUrl(endpoint)
                .build()
            }
        }

        registerPermanentInvokeParam = param => permanentInvokeParams.push(param);

        invoke = (destination, ...params) => this.state.connection.invoke(destination, ...this.state.permanentInvokeParams, ...params);

        on = (destination, func) => this.state.connection.on(destination, func);

        startConnection = () => this.state.connection.start();

        componentWillUnmount()
        {
            this.state.connection.stop();
        }    
    
        render = () =>
        {
            return (
                    <WrappedComponent  />
                );
        }
    }
}