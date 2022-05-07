import React from "react";
<<<<<<< HEAD
import NavigationBar from "../../UI/Navigation/NavigationBar";
import email from './email.png';
=======
import axios from 'axios';

>>>>>>> Working
import "./ConfirmationSent.css";

const ConfirmationSent = () => {
    
    const renderBody = (
<<<<<<< HEAD
        <div className="confirmation-container">
            <div className = "confirmation-notif">
                <p>Your account was successfully created. We've sent a confirmation link to your email. </p>
                <p>Please click the link in your confirmation email to begin using your account.</p>
=======
        <div class="confirmation-container">
            <div class = "confirmation-notif">
                <hr></hr>
                <h1>Welcome to Tresearch</h1>
                <p>A verification link has been sent to your email. The link is only valid for 24 hours.</p>
>>>>>>> Working
            </div>
        </div>
    );

    return (
<<<<<<< HEAD
        <div className="confirmation-sent-component-container">
            <NavigationBar/>
            <div className = "confirmation-email-icon">
                <img src = {email}/>
            </div>
            <div className="confirmation-title">
                <h1 className="confirmation-title-text">You're almost there</h1>
=======
        <div className="component-container">
            <div className="title-text">
                <h1 className="confirmation-title">Confirm your email address</h1>
>>>>>>> Working
            </div>
            {renderBody}
        </div>
    );
}

export default ConfirmationSent;