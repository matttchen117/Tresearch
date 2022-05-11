import React, { useState } from "react";
import jwt_decode from "jwt-decode";

import logo from './logo.png';
import RegistrationPopup from "../Popup/RegistrationPopup";
import LoginForm from "../Popup/LoginPopup";
import Popup from "../../UI/Popup/Popup";
import './NavigationBar.css';
import LoginPopup from "../Popup/LoginPopup";

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
                <li className="logo"><a href="/" ><img src = {logo} alt = "Tresearch Logo"/></a></li>
                <li className="sec-link"><a href="/Features" >Features</a></li>
                <li className="sec-link"><a href="/FAQ" >FAQ</a></li>
                <li className="sec-link"><a href="/Search">Search</a></li>
                <li className="sec"><span onClick={NavToggleSignUp}>Sign up</span></li>
                <li className="sec"><span onClick={NavToggleSignIn}>Sign In</span></li>
            </ul>
        </nav>
    );
   
  return (
    <div className="Home"> 
        {renderNav}
        {isSignUpOpen && <Popup content = {<RegistrationPopup onClick = {NavToggleSignUp}/> } />}
        {isSignInOpen && <Popup content = {<LoginPopup onClick = {NavToggleSignIn}/> } />}
    </div>
  );
}

export default NavigationBar;
