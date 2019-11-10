import React from 'react';
import { Manager, Reference, Popper } from 'react-popper';
import './styles.css';
import TooltipElement from './TooltipElement';

const showClassName = "tooltip-show";
const hideClassName = "tooltip-hide";

export default class Tooltip extends React.PureComponent 
{
  state = {
    renderTooltip: false,
    updateScheduler: null
  }

  renderTooltip = bool => 
  {
    this.setState({renderTooltip: bool});
  }

  render()
  {
    let renderSpecificClass = this.state.renderTooltip ? showClassName : hideClassName;
    
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
          {({ref, style, placement, arrowProps, scheduleUpdate}) => {
            return (
              <TooltipElement parentTransitionPeriod={this.props.parentTransitionPeriod} tooltipref={ref} style={style} arrowProps={arrowProps} placement={placement} scheduleUpdate={scheduleUpdate} tooltip={this.props.tooltip} renderSpecificClass={renderSpecificClass}/>
            )}}
        </Popper>
      </Manager>
    )
  }
};