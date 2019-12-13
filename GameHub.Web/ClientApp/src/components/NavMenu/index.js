import React, { Component } from 'react';
import { UsernameCtx } from  './../../context/Username';
import NavMenu from './NavMenu';

export default class NavMenuWrapper extends Component
{
  render()
  {
    return (
      <div>
        <UsernameCtx.Consumer>
          {context => (
            <NavMenu {...this.props} context={context} />
          )}
        </UsernameCtx.Consumer>
      </div>
    )
  }
}