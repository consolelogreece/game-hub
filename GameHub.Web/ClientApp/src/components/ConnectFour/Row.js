import React from 'react';
import Tile from './Tile';

export default props => {
    let tiles = [];

    for (let i = 0; i < props.cols; i++)
    {
        tiles.push(
            <Tile />
        );
    }

    return (
        <div>
            {tiles}
        </div>
    )
}