import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ConnectFour } from './components/ConnectFour/ConnectFour';
import { NewGame } from './components/ConnectFour/NewGame';
import Chess from './components/Chess/Chess';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout>
            <Switch>
                <Route exact path='/' component={Home} />
                <Route path='/connectfour/createroom' component={NewGame} />
                <Route path='/connectfour/:gameId' component={ConnectFour} />
                <Route path='/chess' component = {Chess} />
            </Switch>
      </Layout>
    );
  }
}
