import { HubConnectionBuilder } from '@aspnet/signalr';

export const Connect = () => {
    return new HubConnectionBuilder()
        .withUrl("/connectfour")
        .build();
}