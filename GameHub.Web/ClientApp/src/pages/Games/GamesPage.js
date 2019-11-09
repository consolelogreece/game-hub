import React, { Component } from 'react';
import axios from 'axios';
import Card  from '../../components/Card/Card';
import Grid from '../../components/Grid/Grid';
import HoverFadeOverlay from '../../components/HoverFadeOverlay';
import { Title } from '../../components/Common/Text';
import Popup from '../../components/Common/Popup';
import ConnectFourForm from '../../forms/ConnectFour';
import ChessForm from '../../forms/Chess';
import './styles.css';

const showFormClass = "game-form-show";
const hideFormClass = "game-form-hide";
const showPopupClass = "game-popup-show";
const hidePopupClass = "game-popup-hide";

export default class Games extends Component {

  constructor(props)
  {
    super(props);
    this.state={
      games:[],
      loading: true,
      imagesLoaded:0,
      formMap: {
        "Connect Four": ConnectFourForm,
        "Chess": ChessForm
      },
      selectedForm:"",
      renderPopup: false
    }
  }
  
  componentDidMount()
  {
    this.props.context.setIsLoading(true);
    
    axios.get("api/games/getgames").then(res => {
      this.setState({
        games:res.data
      })
    })
  }

  onImageLoad = () =>
  {
    this.setState({imagesLoaded: this.state.imagesLoaded + 1}, () => {
        this.updateLoadingStatus()
    });
  }

  updateLoadingStatus()
  {
    if (this.state.imagesLoaded === this.state.games.length && this.state.loading)
    {
        this.props.context.setIsLoading(false)
    }
  }

  componentWillUnmount()
  {
    this.props.context.setIsLoading(true)
  }

  closePopup = () =>
  {
    this.setState({renderPopup: false}, () => {
      setTimeout(() => {
          this.setState({
              selectedForm: ""
          })
      }, 300)
    });
  }

  openPopup = selectedForm =>
  {
    this.setState({renderPopup: true, selectedForm: selectedForm})
  }

  render () {
    let games = this.state.games.map(g => (
      <div style={{cursor: "pointer"}} onClick={() => this.openPopup(g.name)}>
        <HoverFadeOverlay text={g.name} fadeColor={"rgba(0,0,0,0.8)"}>
          <Card {...g} onImageLoad={this.onImageLoad}/> 
        </HoverFadeOverlay>
      </div>
    ));

    let Form = ((selectedForm, formMap) => {
      if (selectedForm === "") return "";
      
      let Form = formMap[selectedForm];

      return <Form history={this.props.history}/>
    })(this.state.selectedForm, this.state.formMap);

    let renderDependentClassForm = this.state.renderPopup ? showFormClass : hideFormClass;
    let renderDependentClassPopup =  this.state.renderPopup ? showPopupClass : hidePopupClass;
    
    return (
      <div>
          {Form !== "" && (
            <div id="games-popup-container">
              <Popup 
                superContainerClassNames={renderDependentClassPopup} 
                containerClassNames={renderDependentClassForm} 
                popupStyles={{width:" 100%", maxWidth: "700px"}} 
                superContainerStyles={{backgroundColor: "rgba(0,0,0, 0.5)"}}
                onClose={() => this.closePopup()}>
                {Form}
              </Popup>
            </div>
          )}
        <div style={{margin: "30px 0px"}}>
          <Title text="Games"/> 
        </div>
        <Grid elements={games} />
      </div>
    );
  }
}