import React, {Component} from 'react';
import Grid from './grid';
import Ship from './ship';
import './styles.scss';

export default class BattleshipsSetupBoard extends Component
{
    constructor(props)
    {
        super(props);

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
                } 
            ]
        }
    }

    handleDrag = event =>
    {
        event.persist()
        let ship = this.state.ships[event.target.id];
    }

    calculateOffsets = (event) => {
        let grid = {}
        let gridLengthPx = this.state.singleSquareDimension;
        let relativeX = event.clientX -  grid.offsetLeft;
        if (relativeX < 0) relativeX = 0;
        
        // minus single square dimension to keep it inside grid, otherwise you'd be able to move the square on the outside
        if (relativeX > gridLengthPx) relativeX = gridLengthPx - this.state.singleSquareDimension;
        
        let relativeY = event.clientY - grid.offsetTop;
        if (relativeY < 0) relativeY = 0;
        if (relativeY > gridLengthPx) relativeY = gridLengthPx - this.state.singleSquareDimension;
        
        let blockX = Math.floor(relativeX / this.state.singleSquareDimension);
        let blockY = Math.floor(relativeY / this.state.singleSquareDimension);
            
        
        // + 1 to center in grid. this will be a different number depending on dimensions of grid/squares
        let trueOffsetX = (blockX * this.state.singleSquareDimension) + 1;
        let trueOffsetY = (blockY * this.state.singleSquareDimension) + 1;
        
        // var squareOccupied = availableDraggables.find(el => {
        //     return el.style.left == `${trueOffsetX}px` && el.style.top == `${trueOffsetY}px` && el != activeDraggable
        // }) != undefined;
        
        // // if (squareOccupied) return;
        
        // activeDraggable.style.left = `${trueOffsetX}px`;
        // activeDraggable.style.top = `${trueOffsetY}px`;

        return {left: trueOffsetX, top: trueOffsetY};
    }

    render()
    {
        let ships = this.state.ships.map((ship, index) => <Ship id={index} handleDrag={this.handleDrag} {...ship} />)

        return(
            <div onDragOver={this.handleDrag}>
                <Grid rows={10} cols={10} styles={{"1,3":{backgroundColor: "pink"}}} >
                    {ships}
                </Grid>
            </div>
        );
    }
}