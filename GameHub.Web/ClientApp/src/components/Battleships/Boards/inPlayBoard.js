import React, {Component} from 'react';
import Grid from './Common/grid';
import Ship from '../Ships/ShipPlay';
import '../styles.scss';

export default class BattleshipsPlayBoard extends Component
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
            squareStyles: {}
        }
    }

    static getDerivedStateFromProps(props, state)
    {
        let nPixelsSquare = props.containerWidth / state.rows;

        if (nPixelsSquare !== state.nPixelsSquare)
        {
            return {
                nPixelsSquare: nPixelsSquare
            }
        }  

        return null;
    }
                           

    render()
    {
        let ships = this.state.ships.map((ship, index) => {
            return (
                <Ship
                    nPixelsSquare={this.state.nPixelsSquare} 
                    id={index}  
                    {...ship}
                />
            )}
        )

        return(
            <div>
                <Grid 
                    width={this.state.nPixelsSquare * this.state.cols}
                    height={this.state.nPixelsSquare * this.state.rows}
                    gridRef={this.gridRef}
                    nPixelsSquare={this.state.nPixelsSquare} 
                    rows={10} 
                    cols={10} 
                    styles={this.state.squareStyles}>
                    {ships}
                </Grid>
            </div>
        );
    }
}