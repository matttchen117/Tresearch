import React from "react";
import axios from 'axios';

import "./ConfirmationSent.css";

const ConfirmationSent = () => {
    
    const renderBody = (
        <div class="confirmation-container">
            <div class = "confirmation-notif">
                <p>Your account was successfully created. Pleae click the link in your confirmation email to begin using your account.</p>
            </div>
        </div>
    );

    return (
        <div className="component-container">
            <div className="title-text">
                <h1 className="confirmation-title">Congradulations!</h1>
            </div>
            {renderBody}
        </div>
    );
}

export default ConfirmationSent;