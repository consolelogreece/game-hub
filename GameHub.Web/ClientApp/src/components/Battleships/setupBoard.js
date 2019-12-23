import React, {Component} from 'react';
import Grid from './grid';
import Ship from './ship';
import './styles.scss';

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
            ships: [
                {
                    orientation: "vertical",
                    x: 0,
                    y: 0,
                    length: 3
                },
                {
                    orientation: "vertical",
                    x: 1 ,
                    y: 0,
                    length: 3
                },

                {
                    orientation: "horizontal",
                    x: 1 ,
                    y: 4,
                    length: 5
                } 
            ],
            selectedShipIndex: -1,
            squareStyles: {}
        }
    }

    handleDrag = event =>
    {
        event.persist()
        let ship = this.state.ships[event.target.id];
    }

    onMouseDown = event =>
    {
        let target = event.target.parentNode;

        if (target.attributes.name == undefined) return;

        let targetName = target.attributes.name.nodeValue;

        if (targetName != "ship") return;
        
        this.setState({selectedShipIndex: target.id});
    }

    onMouseUp = event =>
    {
        this.setState({selectedShipIndex: -1});
    }

    getStyles()
    {
        let overlappingSquares = [];

        let ships = this.state.ships;

        ships.forEach(s1 => {
            ships.forEach(s2 => {
                if (s1 === s2) return;
                overlappingSquares.push(...this.detectOverlap(s1, s2))   
            })
        });

        let newSquareStyles =  {};

        overlappingSquares.forEach(sq => {
            newSquareStyles[`${sq[0]},${sq[1]}`] = {backgroundColor: "red"};
        });

        return newSquareStyles;

        //this.setState({squareStyles: newSquareStyles});
    }

    onMouseMove = event =>
    {
        if (this.state.selectedShipIndex == -1) return;

        let ships = this.state.ships;

        let ship = ships[this.state.selectedShipIndex];

        let {row, col} = this.calculateRelativePositon(event.clientX, event.clientY)

        let offsets = this.calculateOffsets(row, col, ship);

        // if the new position is the same as the old one, no need to update styles etc..
        if (ship.x === offsets.left && ship.y === offsets.top) return;

        ship.x = offsets.left;
        ship.y = offsets.top;

        let styles = this.getStyles();

        this.setState({ships:[...ships], squareStyles: styles})
    }

    calculateRelativePositon(x,y)
    {
        let gridBoundingRect = this.gridRef.current.getBoundingClientRect();

        let gridLeft = gridBoundingRect.left;
        let gridTop = gridBoundingRect.top;

        let relativeX = x - gridLeft;
        
        let relativeY = y - gridTop;
   

        return {row: relativeX, col: relativeY};
    }

    calculateOffsets = (row,col, ship) => {

        let gridLengthPx = this.state.nPixelsSquare * this.state.rows;

        let leftSquareMultiplier = ship.orientation === "horizontal" ? ship.length : 1;
        let topSquareMultiplier = ship.orientation === "vertical" ? ship.length : 1;

        if (row < 0) row = 0;
        // minus single square dimension to keep it inside grid, otherwise you'd be able to move the square on the outside
        if (row > gridLengthPx - (leftSquareMultiplier * this.state.nPixelsSquare)) row = gridLengthPx - (this.state.nPixelsSquare * leftSquareMultiplier);

        if (col < 0) col = 0;
        if (col > gridLengthPx - (topSquareMultiplier * this.state.nPixelsSquare)) col = gridLengthPx - (this.state.nPixelsSquare * topSquareMultiplier);

        let trueOffsetX = Math.floor(row / this.state.nPixelsSquare);
        let trueOffsetY = Math.floor(col / this.state.nPixelsSquare);

        return {left: trueOffsetX, top: trueOffsetY};
    }

    detectOverlap = (ship1, ship2) =>
    {
        let ship1Squares = this.getShipSquares(ship1);

        let ship2Squares = this.getShipSquares(ship2);

        let overlappingSquares = [];

        ship1Squares.forEach(s1 => {
            ship2Squares.forEach(s2 =>
            {
                if (s1[0] == s2[0] && s1[1] == s2[1]) 
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

        let newOffsets = this.calculateOffsets(ship.x *this.state.nPixelsSquare, ship.y * this.state.nPixelsSquare, ship);

        ship.x = newOffsets.left;

        ship.y = newOffsets.top;

        this.setState({ships: ships, squareStyles: styles});
    }

    getShipSquares(ship)
    {
        let newlyOccupiedSquares = [];

        function horizontal(length)
        {
            for (let i = 0; i < length; i++)
            {
                newlyOccupiedSquares.push([ship.x + i, ship.y])
            }
        }

        function vertical(length)
        {
            for (let i = 0; i < length; i++)
            {
                newlyOccupiedSquares.push([ship.x, ship.y + i])
            }
        }
        
        ship.orientation === "horizontal" ? horizontal(ship.length) : vertical(ship.length);

        return newlyOccupiedSquares;
    }

    render()
    {
        let ships = this.state.ships.map((ship, index) => {
            return (
                <Ship style={{cursor: this.state.selectedShipIndex === -1 ? "grab" : "grabbing"}} nPixelsSquare={this.state.nPixelsSquare} id={index} handleDrag={this.handleDrag} {...ship}>
                    <button onClick={() => {this.rotateShip(index)}}>test</button>
                </Ship>
            )}
        )

        return(
            <div>
                <Grid gridRef={this.gridRef} onMouseDown={this.onMouseDown} onMouseUp={this.onMouseUp} onMouseMove={this.onMouseMove} rows={10} cols={10} styles={this.state.squareStyles} >
                    {ships}
                </Grid>
            </div>
        );
    }
}