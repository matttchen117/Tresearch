import React, { useCallback, useState, useEffect } from "react";
import logo from './logo.png';
import front from './header.png';
import Register from "../../Features/Registration/Registration.js";
import NavigationBar from "../../UI/Navigation/NavigationBar";
import Popup from "../../UI/Popup/Popup";
import './Home.css';
import Recover from "../../Features/Recover/Recover";

function Home() {
    

    const renderHeader = (
        <div className = "header-image">
            <img src = {front}></img>
        </div>
    );

   
  return (
    <div className="Home"> 
        {<NavigationBar/>}
        <div className="home-content-container">
            {renderHeader}
        </div>
    </div>
  );
}

export default Home;
