import React, {Component} from 'react';
import Grid from './Common/grid';
import Ship from '../Ships/ShipPlay';
import { gridSquareBlue } from '../../../utils/variables';
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
            ships: []
        }
    }

    static getDerivedStateFromProps(props, state)
    {
        let nPixelsSquare = props.containerWidth / state.rows;

        let ships = props.ships === undefined ? [] : [...props.ships];

        let newState = {};  

        if (nPixelsSquare !== state.nPixelsSquare)
        {
            newState.nPixelsSquare = nPixelsSquare;
        }

        if (state.ships !== ships)
        {
            newState.ships = ships;
        }

        if (Object.keys(newState).length === 0) return null;

        return newState;
    }

    getStylesByBoardState(boardState)
    {
        let styles = {};

        if (boardState === undefined) return styles;

        for(let i = 0; i < boardState.length; i++)
        {
            for (let j = 0; j < boardState[i].length; j++)
            {
                let key = `${i},${j}`;

                if (boardState[i][j] === null) continue;

                switch (boardState[i][j].state)
                {
                    //Untouched
                    case 0:
                        break;

                    // Missed
                    case 1:
                        styles[key] = {
                            background: `radial-gradient(circle, #ffffff 26%, ${gridSquareBlue} 30%)`
                        };
                        break;
                    
                    // Hit
                    case 2:
                        styles[key] = {
                            background: `radial-gradient(circle, #ff0000 26%, ${gridSquareBlue} 30%)`
                        };
                        break;
                }
            }
        }

        return styles;
    }

    formatShip = (unformattedShip, key) => 
    {
        let shipSegmentStyles = [];
        let horizontalMult = unformattedShip.orientation !== "horizontal" ? 1 : 0;
        let verticalMult = unformattedShip.orientation !== "vertical" ? 1 : 0;

        // check to see if any of the squares occupied by the ship are hit, as if they are then the ship is hit. create an array keeping track of hit ship segments and create relevent styles.
        for(let i = 0; i < unformattedShip.length ; i++)
        {
            // as stated earlier, 2 is the numerical representation of a backend enumeration for the status of a square.
            if (this.props.boardState[unformattedShip.row + (i * horizontalMult)][unformattedShip.col + (i * verticalMult)].state === 2)
            {
                shipSegmentStyles.push({backgroundColor:"#D8000C"})
            }
            else
            {
                shipSegmentStyles.push({backgroundColor:"#848482"});
            }
        }
   
        return <Ship
            nPixelsSquare={this.state.nPixelsSquare} 
            id={key}
            key={key}
            {...unformattedShip}
            shipSegmentStyles={shipSegmentStyles}
        />
    }

    render()
    {
        let ships = this.state.ships.map((ship, index) => {
            return (
                this.formatShip(ship, index)
            )}
        )

        let styles = this.getStylesByBoardState(this.props.boardState);

        return(
            <div>
                <Grid 
                    width={this.state.nPixelsSquare * this.state.cols}
                    height={this.state.nPixelsSquare * this.state.rows}
                    gridRef={this.gridRef}
                    nPixelsSquare={this.state.nPixelsSquare} 
                    rows={10} 
                    cols={10} 
                    styles={styles}
                    onSquareClick={this.props.onSquareClick}>
                    {ships}
                    {this.props.children}
                </Grid>
            </div>
        );
    }
}