import React, { Component } from 'react';
import axios from 'axios';
import Card  from './Card/Card'
import Grid from './Grid/Grid'

export default class Games extends Component {

  constructor(props)
  {
    super(props);
    this.state={games:[]}
  }
  componentDidMount()
  {
    axios.get("api/games/getgames").then(res => console.log(this.setState({games:res.data})))
  }

  render () {

    let games = this.state.games.map(g => (
      <Card {...g} />
    ));
    
    return (
      <div>
        <h1>Game Hub</h1>
        <div style={{height: "300px"}}>
          <Grid elements={games} />
        </div>
      </div>
    );
  }
}
