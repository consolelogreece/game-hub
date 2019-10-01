import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Game Hub</h1>
        <a href="/connectfour/createroom">Connect Four</a>
      </div>
    );
  }
}
