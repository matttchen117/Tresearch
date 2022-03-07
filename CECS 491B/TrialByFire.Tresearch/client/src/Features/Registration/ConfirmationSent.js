import React from "react";
import axios from 'axios';

import "./ConfirmationSent.css";

const ConfirmationSent = () => {
    
    const renderBody = (
        <div class="confirmation-container">
            <div class = "confirmation-notif">
                <hr></hr>
                <h1>Welcome to Tresearch</h1>
                <p>A verification link has been sent to your email. The link is only valid for 24 hours.</p>
            </div>
        </div>
    );

    return (
        <div className="component-container">
            <div className="title-text">
                <h1 className="confirmation-title">Confirm your email address</h1>
            </div>
            {renderBody}
        </div>
    );
}

export default ConfirmationSent;