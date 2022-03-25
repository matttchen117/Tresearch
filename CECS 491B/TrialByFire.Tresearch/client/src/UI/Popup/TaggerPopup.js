import React from "react";
import "./TaggerPopup.css"

const TaggerPopup = props => {
    return (
        <div className=".tagger-popup-page">
            <span className =".close-icon-tagger-popup" onClick={props.onClick}>&#10006;</span>
            
        </div>
    )
}

  export default TaggerPopup;
  