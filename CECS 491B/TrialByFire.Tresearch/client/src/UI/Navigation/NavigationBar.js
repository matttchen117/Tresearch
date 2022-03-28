import React, { useCallback, useState, useEffect } from "react";


import logo from './logo.png';
import Register from "../../Features/Registration/Registration.js";
import RegistrationPopup from "../Popup/RegistrationPopup";
import Authentication from "../../Features/Login/Authentication";
import Login from "../../Features/Login/Login";
import Popup from "../../UI/Popup/Popup";
import './NavigationBar.css';

function NavigationBar() {
    const [isSignUpOpen, setIsSignUpOpen] = useState(false);
    const [isSignInOpen, setIsSignInOpen] = useState(false);

    const NavToggleSignUp = () => {
        setIsSignUpOpen(!isSignUpOpen);
        if(isSignInOpen){
            setIsSignInOpen(!isSignInOpen);
        }
    }

    const NavToggleSignIn = () => {
        setIsSignInOpen(!isSignInOpen);
        if(isSignUpOpen){
            setIsSignUpOpen(!isSignUpOpen);
        }
    }

    const renderNav = (
        <nav className = "navbar-container">
           <ul className = "nav-links">
                <li className="logo"><a href="/" ><img src = {logo}/></a></li>
                <li className="sec">About</li>
                <li className="sec"><span onClick={NavToggleSignUp}>Sign up</span></li>
                <li className="sec"><span onClick={NavToggleSignIn}>Sign In</span></li>
            </ul>
        </nav>
    );
   
  return (
    <div className="Home"> 
        {renderNav}
        {isSignUpOpen && <Popup content = {<RegistrationPopup onClick = {NavToggleSignUp}/> } />}
        {isSignInOpen && <Popup content = {<Login/> } handleClose = {NavToggleSignIn}/>}
    </div>
  );
}

export default NavigationBar;
