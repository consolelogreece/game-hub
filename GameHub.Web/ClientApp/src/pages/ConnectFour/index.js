import React from 'react'
import NewGameForm from '../../components/ConnectFour/NewGameForm';
import { Subtitle, Title} from '../../components/Common/Text';

export default props => {
    return (
        <div>
            <Title text="Connect Four" />
            <Subtitle>Summary</Subtitle> 
            <p>
                If you're looking for a simple strategy game that can be played with just about anyone, including young children, Connect Four is for you. Connect Four is a simple game similar to Tic-Tac-Toe. Only instead of three in a row, the winner must connect four in a row.
            </p>
            <Subtitle>How to win</Subtitle>
            <p>
                To win Connect Four you must be the first player to get four of your colored tokens in a row either horizontally, vertically or diagonally. 
            </p>
            <NewGameForm history={props.history}/>
            <Subtitle>Key</Subtitle>
            <p>
                Rows: The amount of rows in your board (max 30).
                Columns: The amount of columns in your board (max 30).
                Win Threshold: The amount of tokens in a row needed to win.
                Maximum Players: The maximum number of players able to join your game.
            </p>
        </div>
    )
}