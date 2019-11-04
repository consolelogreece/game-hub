import React, { Component } from 'react';
import axios from 'axios';
import Card  from '../../components/Card/Card';
import Grid from '../../components/Grid/Grid';
import HoverFadeOverlay from '../../components/HoverFadeOverlay';
import { Title } from '../../components/Common/Text';
import LoadingScreen from '../../components/Common/LoadingScreen';
import ConnectFourForm from '../../forms/ConnectFour';

export default class Games extends Component {

  constructor(props)
  {
    super(props);
    this.state={
      games:[],
      loading: true,
      imagesLoaded:0,
      formMap: {
        "Connect Four": ConnectFourForm
      },
      selectedForm:""
    }
  }
  
  componentDidMount()
  {
    axios.get("api/games/getgames").then(res => {
      this.setState({
        games:res.data
      })
    })
  }

  onImageLoad = () =>
  {
    this.setState({imagesLoaded: this.state.imagesLoaded + 1});
  }
  
  componentDidUpdate()
  {
    this.updateLoadingStatus();
  }

  updateLoadingStatus()
  {
    if (this.state.imagesLoaded === this.state.games.length && this.state.loading)
    {
      this.setState({loading: false})
    }
  }

  render () {
    let games = this.state.games.map(g => (
      <div style={{cursor: "pointer"}} onClick={() => this.props.history.push(g.url)}>
        <HoverFadeOverlay text={g.name} fadeColor={"rgba(0,0,0,0.8)"}>
          <Card {...g} onImageLoad={this.onImageLoad}/> 
        </HoverFadeOverlay>
      </div>
    ));

    return (
      <div>
        <LoadingScreen render={this.state.loading}/>
        <div style={{margin: "30px 0px"}}>
          <Title text="Games"/> 
        </div>
        <Grid elements={games} />
      </div>
    );
  }
}