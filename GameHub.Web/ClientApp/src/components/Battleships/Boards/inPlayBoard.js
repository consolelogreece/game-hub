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
            ships: []
        }
    }

    static getDerivedStateFromProps(props, state)
    {
        let nPixelsSquare = props.width / state.rows;

        let ships = props.ships === undefined ? [] : [...props.ships];

        if (nPixelsSquare !== state.nPixelsSquare || state.ships !== ships)
        {
            return {
                nPixelsSquare: nPixelsSquare,
                ships: ships
            }
        }  

        return null;
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
                            background: "radial-gradient(circle, #ffffff 26%, transparent 30%)"
                        };
                        break;
                    
                    // Hit
                    case 2:
                        styles[key] = {
                            background: "radial-gradient(circle, #ff0000 26%, transparent 30%)"
                        };
                        break;
                }
            }
        }

        return styles;
    }

    render()
    {
        let ships = this.state.ships.map((ship, index) => {
            return (
                <Ship
                    nPixelsSquare={this.state.nPixelsSquare} 
                    id={index}
                    key={index}
                    {...ship}
                    // because backend uses an enum for horizontal/vertical value, we have to convert back to string form here.
                    orientation={ship.orientation === 1 ? "horizontal" : "vertical"}
                />
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
                </Grid>
            </div>
        );
    }
}