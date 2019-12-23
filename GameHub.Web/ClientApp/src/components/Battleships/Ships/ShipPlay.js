import React, {Component} from 'react';
import Body from './Body/ShipBodyPlay';
import Head from './Head/ShipHeadPlay';
import Ship from './Ship';

export default props =>
{   
    return (
        <Ship {...props} Head={Head} Body={Body} />
    ); 
}