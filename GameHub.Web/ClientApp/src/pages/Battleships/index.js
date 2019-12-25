import React, { Component } from 'react';
import { LoadingCtx } from  '../../context/Loading';
import Page from './BattleshipsPage';

export default class BattleshipsWrapper extends Component
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