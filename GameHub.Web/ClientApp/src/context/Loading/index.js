import React, { Component } from 'react';
import LoadingScreen from '../../components/Common/LoadingScreen'

export let LoadingCtx = React.createContext();

export class LoadingProvider extends Component {

    constructor(props) {
      super(props);
      this.state = {
        loading: false
      };
    }

    update = loading => {
       this.setState({loading: loading});
    }

    render() {
        return (
           <LoadingCtx.Provider value={{setIsLoading: this.update}}>      
              <LoadingScreen render={this.state.loading} /> 
              {this.props.children}
           </LoadingCtx.Provider>
       )
    }
}