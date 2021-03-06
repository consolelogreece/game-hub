import React, { Component } from 'react';
import { UsernameCtx } from  '../../context/Username';
import SigninForm from './Signin';

export default class SigninFormWrapper extends Component
{
  render()
  {
    return (
      <div>
        <UsernameCtx.Consumer>
          {context => (
            <SigninForm {...this.props} context={context} />
          )}
        </UsernameCtx.Consumer>
      </div>
    )
  }
}