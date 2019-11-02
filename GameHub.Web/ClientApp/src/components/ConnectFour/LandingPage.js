import React from 'react'
import NewGameForm from './NewGameForm';

export default props => {
    return (
        <div>
            <NewGameForm history={props.history}/>
        </div>
    )
}