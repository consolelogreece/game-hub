import React, { Component } from 'react';
import SetupBoard from './setupBoard';
import InPlayBoard from './inPlayBoard';
import GetRenderedWidthHOC from '../HigherOrder/GetRenderedWidthHOC';
import Button from '../Buttons/Standard';

export default class Battleships extends Component {
    constructor(props) {
        super(props);

        this.state = {
            inPlay: false
        };
    }

    componentDidMount()
    {
        console.log(this.props)
    }

    render()
    {
        let DynamicBoard = GetRenderedWidthHOC(this.state.inPlay ? InPlayBoard : SetupBoard);
        return (
            <div>
                <DynamicBoard />
                <Button onClick={() => this.setState({inPlay: !this.state.inPlay})}>toggle board</Button>
            </div>
        );
    }
}