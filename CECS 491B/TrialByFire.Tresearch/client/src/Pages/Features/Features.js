import React from "react";
import NavBar from "../../UI/Navigation/NavBar";
import "./Features.css";

function Features() {
    

    const renderTagFeatures = (
        <div>

        </div>
    )

    const renderFeatures = (
        <div className = "features-container">
            <h1>Features</h1>
                {renderTagFeatures}
        </div>
    );
  
  return (
    <div className="features-wrapper"> 
        {<NavBar/>}
        <div className="features-content-wrappers">
           {renderFeatures}
        </div>
    </div>
  );
}

export default Features;
