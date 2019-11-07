import { HubConnectionBuilder } from '@aspnet/signalr';
import React, { Component } from 'react';

export default function(WrappedComponent, config)
{
    return class InjectSignalrConnection extends Component
    {
        constructor(props)
        {
            super(props);

            let connection = new HubConnectionBuilder()
            .withUrl(config.hubUrl)
            .build();

            connection.onclose(config.onConnectionClosed);

            this.state = {
                permanentInvokeParams:[],
                connection: connection
            }
        }

        registerPermanentInvokeParam = param => this.setState({permanentInvokeParams: [...this.state.permanentInvokeParams, param]});

        invoke = (destination, ...params) => 
        {
            return this.state.connection.invoke(destination, ...this.state.permanentInvokeParams, ...params)
                .catch(res => 
                {
                    return config.onFail(res);
                });
        }

        on = (destination, func) => 
        {
            if (typeof(destination) === "string")
            {
                this.state.connection.on(destination, func);
            }
            else if(Array.isArray(destination))
            {
                destination.forEach(el => {
                    if (typeof(el) === "string")
                        this.state.connection.on(el, func)
                });
            }
        };

        startConnection = () => 
        {
            return this.state.connection.start()
            .then(() => config.onLoadComplete())
            .catch(() => config.onFail());
        }

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