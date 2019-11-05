import React from 'react';
import Chess from '../../components/Chess/Chess';
import withSignalrConnection from '../../components/HigherOrder/withSignalrConnection';
import ResizeWithContainerHOC from '../../components/HigherOrder/GetRenderedWidthHOC';
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
        let Game = withSignalrConnection(ResizeWithContainerHOC(Chess), `/chesshub${window.location.search}`);
        
        return (
            <div>
                <LoadingScreen render={this.state.loading}/>
                <Game onLoadFail={this.onLoadFail} onLoadComplete={this.onLoadComplete}/>
            </div>
        )
    } 
}