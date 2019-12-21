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
                    orientation: "horizontal",
                    x: 1 ,
                    y: 0,
                    length: 3
                } 
            ],
            selectedShipIndex: -1
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

    onMouseMove = event =>
    {
        if (this.state.selectedShipIndex == -1) return;

        let ships = this.state.ships;

        let offsets = this.calculateOffsets(event, ships[this.state.selectedShipIndex]);

        ships[this.state.selectedShipIndex].x = offsets.left;
        ships[this.state.selectedShipIndex].y = offsets.top;

        this.setState({ships:[...ships]})

        console.log(ships)
    }

    calculateOffsets = (event, ship) => {
        let gridBoundingRect = this.gridRef.current.getBoundingClientRect();

        let gridLeft = gridBoundingRect.left;
        let gridTop = gridBoundingRect.top;

        let leftSquareMultiplier = ship.orientation === "horizontal" ? ship.length : 1;
        let topSquareMultiplier = ship.orientation === "vertical" ? ship.length : 1;

        let gridLengthPx = this.state.nPixelsSquare * this.state.rows ;

        let relativeX = event.clientX - gridLeft;
        if (relativeX < 0) relativeX = 0;
        
        // minus single square dimension to keep it inside grid, otherwise you'd be able to move the square on the outside
        if (relativeX > gridLengthPx - (leftSquareMultiplier * this.state.nPixelsSquare)) relativeX = gridLengthPx - (this.state.nPixelsSquare * leftSquareMultiplier);
        
        let relativeY = event.clientY - gridTop;
        if (relativeY < 0) relativeY = 0;
        if (relativeY > gridLengthPx - (topSquareMultiplier * this.state.nPixelsSquare)) relativeY = gridLengthPx - (this.state.nPixelsSquare * topSquareMultiplier);
        
        let trueOffsetX = Math.floor(relativeX / this.state.nPixelsSquare);
        let trueOffsetY = Math.floor(relativeY / this.state.nPixelsSquare);

        return {left: trueOffsetX, top: trueOffsetY};
    }

    detectCollision = (shipIndex, newOffsets) =>
    {
        let collision = false;

        let ship = this.state.ships[shipIndex];

        let newlyOccupiedSquares = [];

        for (let i = 0; i < ship.length; i++)
        {}

        this.state.ships.forEach((ship, index) =>
        {
            if (shipIndex === index) continue;


        })
    }

    render()
    {
        let ships = this.state.ships.map((ship, index) => <Ship nPixelsSquare={this.state.nPixelsSquare} id={index} handleDrag={this.handleDrag} {...ship} />)

        return(
            <div>
                <Grid gridRef={this.gridRef} onMouseDown={this.onMouseDown} onMouseUp={this.onMouseUp} onMouseMove={this.onMouseMove} rows={10} cols={10} styles={{"1,3":{backgroundColor: "pink"}}} >
                    {ships}
                </Grid>
            </div>
        );
    }
}