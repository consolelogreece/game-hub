import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ConnectFour } from './components/ConnectFour/ConnectFour';
import { NewGameC4 } from './components/ConnectFour/NewGameC4';
import Chess from './components/Chess/Chess';
import { NewGameChess } from './components/Chess/NewGameChess';
import ResizeWithContainerHOC from './components/HigherOrder/GetRenderedWidthHOC';
import withSignalrConnection from './components/HigherOrder/withSignalrConnection';
import gamesPage from './components/Games';
import aboutPage from './components/About';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout>
            <Switch>
                <Route exact path='/' component={Home} />
                <Route path='/games' component={gamesPage} />
                <Route path='/about' component={aboutPage} />
                <Route path='/connectfour/createroom' component={withSignalrConnection(NewGameC4, '/connectfourhub')} />
                <Route path='/connectfour/:gameId' component={withSignalrConnection(ConnectFour, '/connectfourhub')} />
                <Route path='/chess/createroom' component={withSignalrConnection(NewGameChess, '/chesshub')} />
                <Route path='/chess/:gameId' component={withSignalrConnection(ResizeWithContainerHOC(Chess), '/chesshub')} />
            </Switch>
      </Layout>
    );
  }
}
