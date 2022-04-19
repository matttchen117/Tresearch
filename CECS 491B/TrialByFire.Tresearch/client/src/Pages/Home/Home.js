import React, { useCallback, useState, useEffect } from "react";
import front from './header.png';
import NavBar from "../../UI/Navigation/NavBar";


function Home() {
    

    const renderHeader = (
        <div className = "header-image">
            <img src = {front}></img>
        </div>
    );

   
  return (
    <div className="Home"> 
        {<NavBar/>}
        <div className="home-content-container">
            {renderHeader}
        </div>
    </div>
  );
}

export default Home;
