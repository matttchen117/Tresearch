import React, { useCallback, useState, useEffect } from "react";
import logo from './logo.png';
import front from './header.png';
import Register from "../../Features/Registration/Registration.js";
import Popup from "../../UI/Popup/Popup";
import './Home.css';

function Home() {
    const [isSignUpOpen, setIsSignUpOpen] = useState(false);
 

    const toggleSignUp = () => {
        setIsSignUpOpen(!isSignUpOpen);
    }

    

    const renderNav = (
        <nav class = "navbar-container">
           <ul className = "nav-links">
                <li className="logo"><a href="#" ><img src = {logo}/></a></li>
                <li className="sec">About</li>
                <li className="sec"><span onClick={toggleSignUp}>Sign up</span></li>
                <li className="sec"><span>Sign In</span></li>
            </ul>
        </nav>
    );

    const renderHeader = (
        <div class = "header-image">
            <img src = {front} ></img>
        </div>
    );

   
  return (
    <div className="Home"> 
        {renderNav}
        {isSignUpOpen && <Popup content = {<Register/> } handleClose = {toggleSignUp}/>}
        {renderHeader}
    </div>
  );
}

export default Home;
