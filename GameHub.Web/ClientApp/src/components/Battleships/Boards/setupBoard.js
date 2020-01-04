import React, { Component } from 'react';
import Grid from './Common/grid';
import Ship from '../Ships/ShipSetup';
import '../styles.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSyncAlt } from '@fortawesome/free-solid-svg-icons';
import Button from '../../Buttons/Standard';
import {CalculateRelativePositon} from '../../../utils/Helpers/Positioning';

export default class BattleshipsSetupBoard extends Component
{
    constructor(props)
    {
        super(props);

        this.gridRef = React.createRef();

        this.state = {
            rows: 10,
            cols: 10,
            nPixelsSquare: 40,
            ships: [...props.ships],
            selectedShipIndex: -1,
            squareStyles: {}
        }
    }

    static getDerivedStateFromProps(props, state)
    {
        let nPixelsSquare = props.width / state.rows;

        if (nPixelsSquare !== state.nPixelsSquare)
        {
            return {
                nPixelsSquare: nPixelsSquare
            }
        }  

        return null;
    }

    onMouseDown = event =>
    {
        let target = event.target.parentNode;

        if (target.attributes.name === undefined) return;

        let targetName = target.attributes.name.nodeValue;

        if (targetName !== "ship") return;
        
        this.setState({selectedShipIndex: target.id});
    }

    onMouseUp = event =>
    {
        this.setState({selectedShipIndex: -1});
    }

    getOverlappingSquares()
    {
        let overlappingSquares = [];

        let ships = this.state.ships;

        ships.forEach(s1 => {
            ships.forEach(s2 => {
                if (s1 === s2) return;
                overlappingSquares.push(...this.detectOverlap(s1, s2))   
            })
        });

        return overlappingSquares;
    }

    getStyles()
    {
        let newSquareStyles =  {};

        let overlappingSquares = this.getOverlappingSquares();

        overlappingSquares.forEach(sq => {
            newSquareStyles[`${sq[0]},${sq[1]}`] = {backgroundColor: "red"};
        });

        return newSquareStyles;
    }

    onTouchMove = event => 
    {
        this.onMove(event.touches[0].clientX, event.touches[0].clientY);
    }

    onMouseMove = event =>
    {
        this.onMove(event.clientX, event.clientY) 
    }

    onMove = (x, y) => 
    {
        if (this.state.selectedShipIndex === -1) return;

        let ships = this.state.ships;

        let ship = ships[this.state.selectedShipIndex];

        let boundingRect = this.gridRef.current.getBoundingClientRect();

        let {row, col} = CalculateRelativePositon(boundingRect, x, y);

        let offsets = this.calculateOffsets(row, col, ship);

        // if the new position is the same as the old one, no need to update styles etc..
        if (ship.row === offsets.left && ship.col === offsets.top) return;

        ship.col = offsets.col;
        ship.row = offsets.row;

        let styles = this.getStyles();

        this.setState({ships:[...ships], squareStyles: styles})
    }

    calculateOffsets = (row, col, ship) => {

        let gridLengthPx = this.state.nPixelsSquare * this.state.rows;

        let leftSquareMultiplier = ship.orientation === "horizontal" ? ship.length : 1;
        let topSquareMultiplier = ship.orientation === "vertical" ? ship.length : 1;

        if (row < 0) row = 0;
        // minus single square dimension to keep it inside grid, otherwise you'd be able to move the square on the outside
        if (row > gridLengthPx - (topSquareMultiplier * this.state.nPixelsSquare)) row = gridLengthPx - (this.state.nPixelsSquare * topSquareMultiplier);

        if (col < 0) col = 0;
        if (col > gridLengthPx - (leftSquareMultiplier * this.state.nPixelsSquare)) col = gridLengthPx - (this.state.nPixelsSquare * leftSquareMultiplier);

        let trueRow = Math.floor(row / this.state.nPixelsSquare);
        let trueCol = Math.floor(col / this.state.nPixelsSquare);

        return {row: trueRow, col: trueCol};
    }

    detectOverlap = (ship1, ship2) =>
    {
        let ship1Squares = this.getShipSquares(ship1);

        let ship2Squares = this.getShipSquares(ship2);

        let overlappingSquares = [];

        ship1Squares.forEach(s1 => {
            ship2Squares.forEach(s2 =>
            {
                if (s1[0] === s2[0] && s1[1] === s2[1]) 
                {
                    overlappingSquares.push(s1);
                }
            })
        });

        return overlappingSquares;
    }

    rotateShip(index)
    {
        let ships = [...this.state.ships];

        let ship = ships[index];

        ship.orientation = ship.orientation === "vertical" ? "horizontal" : "vertical";

        let styles = this.getStyles();

        let newOffsets = this.calculateOffsets(ship.row *this.state.nPixelsSquare, ship.col * this.state.nPixelsSquare, ship);

        ship.row = newOffsets.row;

        ship.col = newOffsets.col;

        this.setState({ships: ships, squareStyles: styles});
    }

    getShipSquares(ship)
    {
        let newlyOccupiedSquares = [];

        function horizontal(length)
        {
            for (let i = 0; i < length; i++)
            {
                newlyOccupiedSquares.push([ship.row, ship.col + i])
            }
        }

        function vertical(length)
        {
            for (let i = 0; i < length; i++)
            {
                newlyOccupiedSquares.push([ship.row + i, ship.col])
            }
        }
        
        ship.orientation === "horizontal" ? horizontal(ship.length) : vertical(ship.length);

        return newlyOccupiedSquares;
    }

    areShipPositionsValid()
    {
        let overlappingSquares = this.getOverlappingSquares();

        if (overlappingSquares.length > 0) return false;

        return true;
    }

    render()
    {
        let ships = this.state.ships.map((ship, index) => {
            let extragoodstyles = {};

            if (ship.orientation === "vertical")
            {
                extragoodstyles.left = `${- this.state.nPixelsSquare / 3}px`;
                extragoodstyles.top = `${- this.state.nPixelsSquare / 3}px`;
            }
            else
            {
                extragoodstyles.left = `${- this.state.nPixelsSquare / 3}px`;
                extragoodstyles.top = `${- this.state.nPixelsSquare / 3}px`;
            }

            return (
                <Ship status="setup" style={{cursor: this.state.selectedShipIndex === -1 ? "grab" : "grabbing"}} nPixelsSquare={this.state.nPixelsSquare} key={index} id={index} handleDrag={this.handleDrag} {...ship}>
                   <FontAwesomeIcon style={{fontSize: `${this.state.nPixelsSquare / 1.5}px`, ...extragoodstyles}} icon={faSyncAlt} onClick={() => {this.rotateShip(index)}}/>
                </Ship>
            )}
        )

        return(
            <div>
                <Grid 
                    width={this.state.nPixelsSquare * this.state.cols}
                    height={this.state.nPixelsSquare * this.state.rows}
                    gridRef={this.gridRef} 
                    onMouseDown={this.onMouseDown} 
                    onMouseUp={this.onMouseUp} 
                    onMouseMove={this.onMouseMove} 
                    onTouchStart={this.onMouseDown} 
                    onTouchEnd={this.onMouseUp} 
                    onTouchMove={this.onTouchMove}
                    nPixelsSquare={this.state.nPixelsSquare} 
                    rows={10} 
                    cols={10} 
                    styles={this.state.squareStyles} >
                    {ships}
                </Grid>
                <Button onClick={() => this.props.ReadyUp(this.state.ships)}>SUBMIT THE SHIPS</Button>
            </div>
        );
    }
}