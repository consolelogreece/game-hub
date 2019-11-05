import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { ConnectFour } from './components/ConnectFour/ConnectFour';
import Chess from './components/Chess/Chess';
import ResizeWithContainerHOC from './components/HigherOrder/GetRenderedWidthHOC';
import withSignalrConnection from './components/HigherOrder/withSignalrConnection';
import gamesPage from './pages/Games';
import aboutPage from './pages/About';
import './styles.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout>
            <Switch>
                <Route exact path='/about' component={aboutPage} />
                <Route exact path='/connectfour/:gameId' component={withSignalrConnection(ConnectFour, `/connectfourhub${window.location.search}`)} />
                <Route exact path='/chess/:gameId' component={withSignalrConnection(ResizeWithContainerHOC(Chess), `/chesshub${window.location.search}`)} />
                <Route path='/' component={gamesPage} />
            </Switch>
      </Layout>
    );
  }
}