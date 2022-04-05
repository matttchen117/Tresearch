import React from "react";
import "./Popup.css";

const Popup = props => {
    return (
        <div className = "popup-container">
            <div className = "container">
                {props.content}
            </div>
        </div>
    )
}

export default Popup;