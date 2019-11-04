import React from 'react';
import { Manager, Reference, Popper } from 'react-popper';
import './styles.css';

const showClassName = "tooltip-show";
const hideClassName = "tooltip-hide";

export default class Tooltip extends React.PureComponent 
{
  state = {
    renderTooltip: false
  }

  renderTooltip = bool => 
  {
    this.setState({renderTooltip: bool});
  }

  render()
  {
    let renderSpecificClass = this.state.renderTooltip ? showClassName : hideClassName;
    console.log(renderSpecificClass, "specific");
    return (
      <Manager>
        <Reference>
          {({ ref }) => (
            <div ref={ref} id="tooltip-ref-element" onMouseEnter={() => this.renderTooltip(true)} onMouseLeave={() => this.renderTooltip(false)}>
              {this.props.text}
            </div>
          )}
        </Reference>
        <Popper placement="top">
          {({ref, style, placement, arrowProps}) => {
            return (
            <div ref={ref} id="tooltip-pop-element-body" className={renderSpecificClass} style={{...style, }} data-placement={placement}>
              {this.props.tooltip}
              <div ref={arrowProps.ref} id="tooltip-pop-element-arrow" className={renderSpecificClass} style={{...arrowProps.style}} />
            </div>
          )}}
        </Popper>
      </Manager>
    )
  }
};