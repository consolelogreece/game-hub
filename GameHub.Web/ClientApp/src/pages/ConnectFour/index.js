import React from 'react';
import ConnectFour from '../../components/ConnectFour/ConnectFour';
import withSignalrConnection from '../../components/HigherOrder/withSignalrConnection'
import LoadingScreen from '../../components/Common/LoadingScreen';

export default class ConnectFourPage extends React.PureComponent
{
    state={
        loading: true,
        game: null,
        ls: null
    }

    componentDidMount()
    {
        let game = withSignalrConnection(ConnectFour,{
            hubUrl: `/connectfourhub${window.location.search}`, 
            onFail: this.onFail, 
            onConnectionClose: this.onConnectionClosed,
            onLoadComplete: this.onLoadComplete
        });

        this.setState({game: game})
    }

    onLoadComplete = () =>
    {
        this.setState({loading: false});
    }

    onFail = async res => 
    {
        await this.props.history.push("/");
        throw res;
    }

    onConnectionClosed = async () =>
    {
        await this.props.history.push("/");
        throw "connection closed";
    }

    render()
    {
        return (
            <div>
                <LoadingScreen render={this.state.loading}/>
                {!!this.state.game && <this.state.game />}
            </div>
        )
    } 
}