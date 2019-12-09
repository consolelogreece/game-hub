import React, { Component } from 'react';
import axios from 'axios';

export default class about extends Component {
  render () {
    return (
      <div>
        <p>
            Real time games, no signup.
        </p>
        <button onClick={() => {
          axios.post('/api/auth/signup', "test");
        }}>go</button>
      </div>
    );
  }
}
