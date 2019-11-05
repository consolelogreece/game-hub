import React from 'react';
import ConnectFour from '../../components/ConnectFour/ConnectFour';
import withSignalrConnection from '../../components/HigherOrder/withSignalrConnection'
import LoadingScreen from '../../components/Common/LoadingScreen';

export default class ConnectFourPage extends React.PureComponent
{
    state={
        loading: true
    }

    onLoadComplete = () =>
    {
       this.setState({loading: false});
    }

    onLoadFail = res => 
    {
        console.log(res);
        this.props.history.push("/");
    }

    render()
    {
        let Game = withSignalrConnection(ConnectFour, `/connectfourhub${window.location.search}`);
        
        return (
            <div>
                <LoadingScreen render={this.state.loading}/>
                <Game onLoadFail = {this.onLoadFail} onLoadComplete={this.onLoadComplete}/>
            </div>
        )
    } 
}