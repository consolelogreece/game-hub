import React from 'react'
import NewGameForm from '../../components/ConnectFour/NewGameForm';
import { Subtitle, Title} from '../../components/Common/Text';
import Popup from '../../components/Common/Popup';

export default class ConnectFourLandingPage extends React.PureComponent {
    constructor(props)
    {
        super(props);
        this.state = {
            displayPopup: false
        }
    }

    togglePopup()
    {
        this.setState({displayPopup: !this.state.displayPopup});
    }

    render()
    {
        return (
            <div>
                <Title text="Connect Four" />
                <Subtitle style={{marginTop: "40px"}}>How to win</Subtitle>
                <p>
                    To win Connect Four you must be the first player to get four of your colored tokens in a row either horizontally, vertically or diagonally. 
                </p>
                { this.state.displayPopup && (
                    <Popup onClose={this.togglePopup}>
                        <NewGameForm history={this.props.history}/>
                    </Popup> 
                )}
                <button onClick={() => this.togglePopup()}>
                    Create Game
                </button>
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
}