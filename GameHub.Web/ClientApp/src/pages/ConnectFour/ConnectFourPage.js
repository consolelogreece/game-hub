import React from 'react';
import ConnectFour from '../../components/ConnectFour/ConnectFour';
import withSignalrConnection from '../../components/HigherOrder/withSignalrConnection';
import { LoadingCtx } from '../../context/Loading';

export default class ConnectFourPage extends React.PureComponent
{
    state={
        game: null
    }

    componentDidMount()
    {
        this.props.context.setIsLoading(true);

        let game = withSignalrConnection(ConnectFour,{
            hubUrl: `/connectfourhub${window.location.search}`, 
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

    onConnectionClosed = () =>
    {
        this.props.history.push("/");
    }

    componentWillUnmount()
    {
        this.props.context.setIsLoading(true);
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