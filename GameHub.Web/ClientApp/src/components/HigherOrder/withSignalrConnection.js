import { HubConnectionBuilder } from '@aspnet/signalr';
import React, { Component } from 'react';

export default function(WrappedComponent, config)
{
    return class InjectSignalrConnection extends Component
    {
        _connected = false;

        constructor(props)
        {
            super(props);

            let connection = new HubConnectionBuilder()
            .withUrl(config.hubUrl)
            .build();

            connection.onclose(this.onConnectionClosed);

            this.state = {
                permanentInvokeParams:[],
                connection: connection
            }
        }

        registerPermanentInvokeParam = param => this.setState({permanentInvokeParams: [...this.state.permanentInvokeParams, param]});

        invoke = (destination, ...params) => 
        {
            if (this._connected)
            {
                return this.state.connection.invoke(destination, ...this.state.permanentInvokeParams, ...params);
            }

            throw new Error("No Signalr connection");
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

        startConnection = async () => 
        {
            var x = await this.state.connection.start();

            if (x === undefined)
            {
                this._connected = true;
            }
            else
            {
                config.onFail();
                this._connected = false;
            }

            return new Promise((resolve, reject) => { 
                this._connected ? resolve() : reject();
            });
        }

        componentWillUnmount()
        {
            this._connected = false;
            this.state.connection.stop();
        }    

        onConnectionClosed = () =>
        {
            this._connected = false;
            config.onConnectionClosed();
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