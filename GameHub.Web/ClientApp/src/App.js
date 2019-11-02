import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { ConnectFour } from './components/ConnectFour/ConnectFour';
import ConnectFourLandingPage from './components/ConnectFour/LandingPage'
import Chess from './components/Chess/Chess';
import { NewGameChess } from './components/Chess/NewGameChess';
import ResizeWithContainerHOC from './components/HigherOrder/GetRenderedWidthHOC';
import withSignalrConnection from './components/HigherOrder/withSignalrConnection';
import gamesPage from './components/Games';
import aboutPage from './components/About';

export default class App extends Component {
  static displayName = App.name;

  render () {
    console.log(window.location.search)
    return (
        <Layout>
            <Switch>
                <Route exact path='/about' component={aboutPage} />
                <Route exact path='/connectfour' component={ConnectFourLandingPage} />
                <Route exact path='/connectfour/:gameId' component={withSignalrConnection(ConnectFour, `/connectfourhub${window.location.search}`)} />
                <Route exact path='/chess/createroom' component={NewGameChess} />
                <Route exact path='/chess/:gameId' component={withSignalrConnection(ResizeWithContainerHOC(Chess), `/chesshub${window.location.search}`)} />
                <Route path='/' component={gamesPage} />
            </Switch>
      </Layout>
    );
  }
}