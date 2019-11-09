import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import gamesPage from './pages/Games';
import aboutPage from './pages/About';
import ConnectFourPage from './pages/ConnectFour'
import ChessPage from './pages/Chess';
import './styles.css';
import { LoadingProvider } from  './context/Loading';


export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout>
            <LoadingProvider>
              <Switch>
                  <Route exact path='/about' component={aboutPage} />
                  <Route exact path='/connectfour/:gameId' component={ConnectFourPage} />
                  <Route exact path='/chess/:gameId' component={ChessPage} />
                  <Route path='/' component={gamesPage} />
              </Switch>
            </LoadingProvider>
      </Layout>
    );
  }
}