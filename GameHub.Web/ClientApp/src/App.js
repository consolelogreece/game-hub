import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import gamesPage from './pages/Games';
import aboutPage from './pages/About';
import ConnectFourPage from './pages/ConnectFour'
import BattleshipsPage from './pages/Battleships';
import ChessPage from './pages/Chess';
import './styles.css';
import { LoadingProvider } from  './context/Loading';
import { UsernameProvider } from './context/Username';

import battleships from './components/Battleships/setupBoard';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <UsernameProvider>
        <Layout>
          <LoadingProvider>
            <Switch>
                <Route exact path='/about' component={aboutPage} />
                <Route exact path='/connectfour/:gameId' component={ConnectFourPage} />
                <Route exact path='/chess/:gameId' component={ChessPage} />
                <Route exact path='/battleships/:gameId' component={BattleshipsPage} />
                <Route path='/' component={gamesPage} />
            </Switch>
          </LoadingProvider>
        </Layout>
      </UsernameProvider>
    );
  }
}