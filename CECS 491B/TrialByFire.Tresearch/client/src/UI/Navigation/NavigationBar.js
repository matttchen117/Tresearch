import React, { useCallback, useState, useEffect } from "react";
import logo from './logo.png';
import Register from "../../Features/Registration/Registration.js";
import Popup from "../../UI/Popup/Popup";
import './NavigationBar.css';

function NavigationBar() {
    const [isSignUpOpen, setIsSignUpOpen] = useState(false);
 

    const NavtoggleSignUp = () => {
        setIsSignUpOpen(!isSignUpOpen);
    }

    

    const renderNav = (
        <nav class = "navbar-container">
           <ul className = "nav-links">
                <li className="logo"><a href="#" ><img src = {logo}/></a></li>
                <li className="sec">About</li>
                <li className="sec"><span onClick={NavtoggleSignUp}>Sign up</span></li>
                <li className="sec"><span>Sign In</span></li>
            </ul>
        </nav>
    );
   
  return (
    <div className="Home"> 
        {renderNav}
        {isSignUpOpen && <Popup content = {<Register/> } handleClose = {NavtoggleSignUp}/>}
    </div>
  );
}

export default NavigationBar;
