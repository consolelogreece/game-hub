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

        registerPermanentInvokeParam = param => this.setState({permanentInvokeParams: [...this.state.permanentInvokeParams, param]});

        invoke = (destination, ...params) => this.state.connection.invoke(destination, ...this.state.permanentInvokeParams, ...params);

        on = (destination, func) => 
        {
            switch (typeof(destination))
            {
                case "array":
                    destination.forEach(el => {
                        if (typeof(el) == "string")
                            this.state.connection.on(el, func)
                    });

                case "string":
                    this.state.connection.on(el, func);
                    break;
            }
        };

        startConnection = () => this.state.connection.start();

        componentWillUnmount()
        {
            this.state.connection.stop();
        }    
    
        render = () =>
        {
            return (
                    <WrappedComponent 
                        {...this.props}
                        on={this.on} 
                        registerPermanentInvokeParam={this.registerPermanentInvokeParam} 
                        invoke={this.invoke}
                        startConnection={this.startConnection}
                    />
                );
        }
    }
}