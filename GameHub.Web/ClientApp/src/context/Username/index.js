import React, { Component } from 'react';

export let UsernameCtx = React.createContext();

export class UsernameProvider extends Component {

    constructor(props) {
      super(props);
      this.state = {
        username: null
      };
    }

    update = username => {
       this.setState({username: username});
    }

    render() {
        return (
           <UsernameCtx.Provider value={{username: this.state.username, setUsername: this.update}}>   
              {this.props.children}
           </UsernameCtx.Provider>
       )
    }
}