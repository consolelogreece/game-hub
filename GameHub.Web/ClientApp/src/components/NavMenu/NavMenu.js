import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import Signin from '../../forms/Signin';
import './NavMenu.css';

export default class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      displayChooseUsernameForm: false
    };
  }

  toggleChooseUsername()
  {
    this.setState({displayChooseUsernameForm: !this.state.displayChooseUsernameForm})
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render () {
    let usernameItem = this.props.context.username == null ? (
      <NavLink style={{cursor:"pointer"}} onClick={() => this.toggleChooseUsername()} className="text-dark">Choose username</NavLink>
    ) : (
      <NavLink className="text-dark">{this.props.context.username}</NavLink>
    );

    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" style={{zIndex:3}} light>
          <Container>
            <NavbarBrand tag={Link} to="/">GameHub</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/games">Games</NavLink>
                </NavItem>
                <NavItem>
                  {usernameItem}
                </NavItem>
              </ul>
            </Collapse>
          </Container>
        </Navbar>
        {this.state.displayChooseUsernameForm && (
          <Signin toggle={() => this.toggleChooseUsername()} render={this.state.displayChooseUsernameForm} />
        )}
      </header>
    );
  }
}
