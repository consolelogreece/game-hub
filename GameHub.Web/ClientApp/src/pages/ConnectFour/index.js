import React, { Component } from 'react';
import { LoadingCtx } from  '../../context/Loading';
import Page from './ConnectFourPage';

export default class ConnectFourPageWrapper extends Component
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