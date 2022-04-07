import React, { useCallback, useState, useEffect } from "react";
import logo from './logo.png';
import { ContextMenu, ContextMenuTrigger, MenuItem, showMenu } from "react-contextmenu";

import './AuthenticatedNavBar.css';

function AuthenticatedNavBar() {

    const renderProfile = (e) => {
      const initial = "P";  //Replace this later with initial from token
      
      

      return initial;
    }

    const handleLeftMouseClick = (e) => {
      const x = window.innerWidth-10;
      const y = 70;
      console.log(y);
      showMenu({
        position: {x, y},
        id: "contextmenu"
      });
    }    

    const renderMenu = (
        <div className = "nav-profile">
            <ContextMenuTrigger id = "contextmenu">
              <div className = "profile" onClick = {handleLeftMouseClick}>
                {renderProfile()}
              </div>
            </ContextMenuTrigger>
            <ContextMenu id = "contextmenu" className = "nav-context-menu">
              <MenuItem data="contextmenu-item">Settings</MenuItem>
              <MenuItem data="contextmenu-item">Logout</MenuItem>
            </ContextMenu>
        </div>
    );
    

    const renderNav = (
        <nav className = "authenticated-navbar-container">
           <ul className = "nav-links">
                <li className="logo"><a href="/Portal" ><img src = {logo}/></a></li>
                <li className="profile-container"><span>{renderMenu}</span></li>
            </ul>
        </nav>
    );
   
  return (
    <div className="authenticated-nav-bar-wrapper"> 
        {renderNav}
    </div>
  );
}

export default AuthenticatedNavBar;
