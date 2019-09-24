import React, { Component } from 'react';

export default class JoinGame extends Component
{   
    constructor(props)
    {
        super(props);

        this.state = {
            text:""
        }

    }

    HandleChange(e)
    {
        this.setState({...this.state, [e.target.name]: e.target.value})
    }

    JoinGame()
    {
        var name = this.state.text;

        this.props.JoinGame(name);
    }

    render()
    {
        return(
            <div style={{margin: "15px auto 0px auto", textAlign: "center"}}>
                <h6>{this.props.title}</h6>
                <input  
                    name="text" 
                    value={this.state.playerNick} 
                    onChange={e => this.HandleChange(e)}
                    style={{
                        width:"100%",
                        border: "2px solid #aaa",
                        borderRadius: "4px",
                        margin: "8px 0",
                        padding: "8px"
                    }}/>
                <br /> 
                <button 
                style={{
                    width:"100%",
                    border: "2px solid #aaa",
                    borderRadius: "4px",
                    margin: "8px 0",
                    padding: "8px"
                }}
                onClick={() => this.JoinGame()}>Join</button>
            </div>
        )
    }
}