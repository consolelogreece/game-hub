import React, { Component } from 'react';
import Page from './AboutPage';

export default class AboutPageWrapper extends Component
{
  render()
  {
    return (
      <Page {...this.props} />
    )
  }
}