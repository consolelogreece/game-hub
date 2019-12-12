import React from 'react';
import Chess from '../../components/Chess/Chess';
import withSignalrConnection from '../../components/HigherOrder/withSignalrConnection';
import ResizeWithContainerHOC from '../../components/HigherOrder/GetRenderedWidthHOC';
import withTimedErrorMessage from '../../components/HigherOrder/withTimedErrorMessage';

export default class ConnectFourPage extends React.PureComponent
{
    state={
        game: null
    }

    componentDidMount()
    {
        this.props.context.setIsLoading(true);

        let game = withSignalrConnection(ResizeWithContainerHOC(withTimedErrorMessage(Chess)), {
            hubUrl: `/chesshub${window.location.search}`, 
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