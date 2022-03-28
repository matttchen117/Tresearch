import React from "react";
import axios from 'axios';

import "./RecoveryResend.css";

const RecoveryResend = () => {
    const renderBody = (
        <div className="recovery-resend-container">
            <div className = "recovery-resend-notif">
                <p>Your account was successfully created. Pleae click the link in your confirmation email to begin using your account.</p>
            </div>
        </div>
    );

    return (
        <div className="recovery-resend-container">
            <div className="title-text">
                <h1 className="recovery-resend-title">Congradulations!</h1>
            </div>
            {renderBody}
        </div>
    );
}

export default RecoveryResend;