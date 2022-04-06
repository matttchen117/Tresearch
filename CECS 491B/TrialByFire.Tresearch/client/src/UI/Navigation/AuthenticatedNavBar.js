import React, { useCallback, useState, useEffect } from "react";
import logo from './logo.png';

import './AuthenticatedNavBar.css';

function AuthenticatedNavBar() {
    

    const renderNav = (
        <nav className = "navbar-container">
           <ul className = "nav-links">
                <li className="logo"><a href="/Portal" ><img src = {logo}/></a></li>
                <li className="sec"><>Profile</></li>
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
