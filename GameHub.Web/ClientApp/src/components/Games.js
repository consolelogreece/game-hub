import React, { Component } from 'react';

export default class Games extends Component {
  render () {
    return (
      <div>
        <h1>Game Hub</h1>
        <a href="/connectfour/createroom">Connect Four</a>
        <a href="/chess/createroom">Chess</a>
      </div>
    );
  }
}
