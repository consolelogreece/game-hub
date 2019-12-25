import React, { Component } from 'react';
import SetupBoard from './setupBoard';
import GetRenderedWidthHOC from '../HigherOrder/GetRenderedWidthHOC';

export default class Battleships extends Component {
    constructor(props) {
        super(props);

        this.state = {
        };
    }

    componentDidMount()
    {
        console.log(this.props)
    }

    render()
    {
        let DynamicBoard = GetRenderedWidthHOC(SetupBoard);
        return <DynamicBoard />
    }
}