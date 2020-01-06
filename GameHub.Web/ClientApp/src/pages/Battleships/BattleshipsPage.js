import React from 'react';
import Battleships from '../../components/Battleships/Battleships';
import withSignalrConnection from '../../components/HigherOrder/withSignalrConnection';
import withTimedErrorMessage from '../../components/HigherOrder/withTimedErrorMessage';
import GetRenderedWidthHOC from '../../components/HigherOrder/GetRenderedWidthHOC';

export default class ConnectFourPage extends React.PureComponent
{
    state={
        game: null
    }

    componentDidMount()
    {
        this.props.context.setIsLoading(true);

        let game = withSignalrConnection(GetRenderedWidthHOC(withTimedErrorMessage(Battleships)), {
            hubUrl: `/battleshipshub${window.location.search}`, 
            onFail: this.onFail, 
            onConnectionClosed: this.onConnectionClosed,
            onLoadComplete: this.onLoadComplete
        });

        this.setState({game: game})
    }

    onLoadComplete = () =>
    {
        this.props.context.setIsLoading(false);
    }

    onFail = res => 
    {
        this.props.history.push("/");
    }

    onConnectionClosed = res =>
    {
        this.props.history.push("/");
    }

    componentWillUnmount()
    {
        this.props.context.setIsLoading(false);
    }

    render()
    {
        return (
            <div>
                {!!this.state.game && (
                    <this.state.game onLoadComplete={this.onLoadComplete} />
                )}
            </div>
        )
    } 
}