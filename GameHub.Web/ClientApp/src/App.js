import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ConnectFour } from './components/ConnectFour/ConnectFour';
import { NewGameC4 } from './components/ConnectFour/NewGameC4';
import Chess from './components/Chess/Chess';
import { NewGameChess } from './components/Chess/NewGameChess';
import ResizeWithWindowHOC from './components/Common/ResizeWithWindowHOC';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout>
            <Switch>
                <Route exact path='/' component={Home} />
                <Route path='/connectfour/createroom' component={NewGameC4} />
                <Route path='/connectfour/:gameId' component={ConnectFour} />
                <Route path='/chess/createroom' component={ResizeWithWindowHOC(NewGameChess)} />
                <Route path='/chess/:gameId' component={Chess} />
            </Switch>
      </Layout>
    );
  }
}
