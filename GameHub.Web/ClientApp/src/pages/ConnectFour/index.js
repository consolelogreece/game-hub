import React from 'react'
import NewGameForm from '../../components/ConnectFour/NewGameForm';

export default props => {
    return (
        <div>
            <h6>Connect Four</h6>
            <NewGameForm history={props.history}/>
        </div>
    )
}