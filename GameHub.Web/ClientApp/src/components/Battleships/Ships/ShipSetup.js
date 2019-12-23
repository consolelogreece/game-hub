import React, {Component} from 'react';
import Body from './Body/ShipBodySetup';
import Head from './Head/ShipHeadSetup';
import Ship from './Ship';

export default props =>
{   
    return (
        <Ship {...props} Head={Head} Body={Body} />
    ); 
}