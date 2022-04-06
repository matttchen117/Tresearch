import React from "react";
import NavigationBar from "../../UI/Navigation/NavigationBar";
import email from './email.png';
import "./ConfirmationSent.css";

const ConfirmationSent = () => {
    
    const renderBody = (
        <div className="confirmation-container">
            <div className = "confirmation-notif">
                <p>Your account was successfully created. We've sent a confirmation link to your email. </p>
                <p>Please click the link in your confirmation email to begin using your account.</p>
            </div>
        </div>
    );

    return (
        <div className="confirmation-sent-component-container">
            <NavigationBar/>
            <div className = "confirmation-email-icon">
                <img src = {email}/>
            </div>
            <div className="confirmation-title">
                <h1 className="confirmation-title-text">You're almost there</h1>
            </div>
            {renderBody}
        </div>
    );
}

export default ConfirmationSent;