import React, { Component } from 'react';
import axios from 'axios';
import Card  from './Card/Card';
import Grid from './Grid/Grid';
import HoverFadeOverlay from './HoverFadeOverlay';
import { Title } from './Common/Text'
import LoadingScreen from './Common/LoadingScreen'

export default class Games extends Component {

  constructor(props)
  {
    super(props);
    this.state={
      games:[],
      loading: true,
      imagesLoaded:0
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
      <div style={{cursor: "pointer"}} onClick={() => this.props.history.push(g.url + "/createroom")}>
        <HoverFadeOverlay text={g.name} fadeColor={"rgba(0,0,0,0.8)"}>
          <Card {...g} onImageLoad={this.onImageLoad}/> 
        </HoverFadeOverlay>
      </div>
    ));

    return (
      <div>
        { <LoadingScreen classNames={this.state.loading ? "" : "loading-fade-out"}/>}
        <div style={{margin: "30px 0px"}}>
          <Title text="Games"/> 
        </div>
        <Grid elements={games} />
      </div>
    );
  }
}