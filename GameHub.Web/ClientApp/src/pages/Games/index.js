import React, { Component } from 'react';
import { LoadingCtx } from  '../../context/Loading';
import Page from './GamesPage';

export default class GamesPageWrapper extends Component
{
  render()
  {
    return (
      <div>
        <LoadingCtx.Consumer>
          {context => (
            <Page {...this.props} context={context} />
          )}
        </LoadingCtx.Consumer>
      </div>
    )
  }
}