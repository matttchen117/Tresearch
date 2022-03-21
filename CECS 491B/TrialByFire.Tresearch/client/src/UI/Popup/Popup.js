import React from "react";
import "./Popup.css";

const Popup = props => {
    return (
        <div className = "popup-container">
            <div className = "container">
                <span className ="close-icon" onClick = {props.handleClose}>&#10006;</span>
                {props.content}
            </div>
        </div>
    )
}

export default Popup;