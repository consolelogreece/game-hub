import React from 'react';
import './styles.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus, faMinus } from '@fortawesome/free-solid-svg-icons'

export default class IncrementalInput extends React.PureComponent 
{
    onDecrement = () =>
    {
        if (this.props.max === null || this.props.value - this.props.increment < this.props.min) return;

        this.updateState(this.props.value - this.props.increment);  
    }

    onIncrement = () =>
    {
        if (this.props.min === null || this.props.value + this.props.increment > this.props.max) return;

        this.updateState(this.props.value + this.props.increment);
    }

    updateState(val)
    {
        this.setState({value: val}, () => this.props.onValueChange(
        {
            value:val,
            origin: this.props.name
        }));
    }

    render() {
        return (
            <div className="incremental-container">
                <div className="value-button" id="decrease" onClick={this.onDecrement}><FontAwesomeIcon icon={faMinus} /></div>
                <input 
                    className="incremental-input" 
                    type="number" 
                    name={this.props.name}
                    readOnly
                    value={this.props.value}
                />
                <div className="value-button" id="increase" onClick={this.onIncrement}><FontAwesomeIcon icon={faPlus} /></div>
            </div>
        )
    }
}