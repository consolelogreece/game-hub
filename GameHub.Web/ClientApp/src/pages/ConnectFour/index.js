import React from 'react'
import NewGameForm from '../../components/ConnectFour/NewGameForm';
import { Subtitle, Title} from '../../components/Common/Text';

export default props => {
    return (
        <div>
            <Title text="Connect Four" />
            <Subtitle style={{marginTop: "40px"}}>How to win</Subtitle>
            <p>
                To win Connect Four you must be the first player to get four of your colored tokens in a row either horizontally, vertically or diagonally. 
            </p>
            <NewGameForm history={props.history}/>
            <Subtitle style={{marginTop: "40px"}}>Key</Subtitle>
            <p>
                Rows: The amount of rows in your board (max 30). <br />
                Columns: The amount of columns in your board (max 30). <br />
                Win Threshold: The amount of tokens in a row needed to win. <br />
                Maximum Players: The maximum number of players able to join your game.
            </p>
        </div>
    )
}