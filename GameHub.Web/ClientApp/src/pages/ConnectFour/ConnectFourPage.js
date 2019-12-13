import React from 'react';
import ConnectFour from '../../components/ConnectFour/ConnectFour';
import withSignalrConnection from '../../components/HigherOrder/withSignalrConnection';
import withTimedErrorMessage from '../../components/HigherOrder/withTimedErrorMessage';

export default class ConnectFourPage extends React.PureComponent
{
    state={
        game: null
    }

    componentDidMount()
    {
        this.props.context.setIsLoading(true);

        let game = withSignalrConnection(withTimedErrorMessage(ConnectFour), {
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

    onConnectionClosed = res =>
    {
        console.log(";AJDA;LJD;LKJSA;LKJF", res)
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