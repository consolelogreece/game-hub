import React, { Component } from 'react';
import axios from 'axios';
import Card  from './Card/Card';
import Grid from './Grid/Grid';
import HoverFadeOverlay from './HoverFadeOverlay';
import { Title } from './Common/Text'

export default class Games extends Component {

  constructor(props)
  {
    super(props);
    this.state={games:[]}
  }
  
  componentDidMount()
  {
    axios.get("api/games/getgames").then(res => {
      this.setState({games:res.data})
    })
  }

  render () {
    let games = this.state.games.map(g => (
      <div onClick={() => this.props.history.push(g.url + "/createroom")}>
        <HoverFadeOverlay text={g.name} fadeColor={"rgba(0,0,0,0.6)"}>
          <Card thumbnail={g.thumbnail}/> 
        </HoverFadeOverlay>
      </div>
    ));
    
    return (
      <div>
        <div style={{margin: "30px 0px"}}>
          <Title text="Games"/> 
        </div>
        <Grid elements={games} />
      </div>
    );
  }
}