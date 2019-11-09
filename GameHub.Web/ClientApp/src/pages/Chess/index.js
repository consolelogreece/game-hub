import React from 'react';
import Chess from '../../components/Chess/Chess';
import withSignalrConnection from '../../components/HigherOrder/withSignalrConnection';
import ResizeWithContainerHOC from '../../components/HigherOrder/GetRenderedWidthHOC';
import LoadingScreen from '../../components/Common/LoadingScreen';

export default class ConnectFourPage extends React.PureComponent
{
    state={
        loading: true,
        game: null
    }

    componentDidMount()
    {
        let game = withSignalrConnection(ResizeWithContainerHOC(Chess), {
            hubUrl: `/chesshub${window.location.search}`, 
            onFail: this.onFail, 
            onConnectionClosed: this.onConnectionClosed,
            onLoadComplete: this.onLoadComplete
        });

        this.setState({game: game})
    }

    onLoadComplete = () =>
    {
        this.setState({loading: false});
    }

    onFail = res => 
    {
        this.props.history.push("/");
    }

    onConnectionClosed = () =>
    {
        this.props.history.push("/");
    }

    render()
    {   
        return (
            <div>
                <LoadingScreen render={this.state.loading}/>
                {!!this.state.game && <this.state.game onLoadComplete={this.onLoadComplete} />}
            </div>
        )
    } 
}