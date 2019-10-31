import React, { Component } from 'react';
import axios from 'axios';

export default class Games extends Component {
  render () {
    axios.get("api/games/getgames").then(res => console.log(res.data))
    return (
      <div>
        <h1>Game Hub</h1>
        <a href="/connectfour/createroom">Connect Four</a>
        <a href="/chess/createroom">Chess</a>
      </div>
    );
  }
}
