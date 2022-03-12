import React from "react";
import axios from 'axios';

import "./InactiveLink.css";

const InactiveLink = () => {
    
    const handleSubmit = (event) => {
        event.preventDefault();
        var { username, password } = document.forms[0];

        axios.post('https://localhost:44303/Registration/register?=', {username, password})
        .then(res => {
            
         })
    };

    const renderBody = (
        <div class="inactive-container">
            <div class = "inactive-notif">
                <hr></hr>
                <h1>Uh Oh! Looks like your link expired.</h1>
                <div className="email-button-container">
                        <input type="submit" value="Resend Verification link "  onSubmit={handleSubmit}/>
                    </div>
            </div>
        </div>
    );

    return (
        <div className="component-container">
            <div className="title-text">
                <h1 className="inactive-title">Email confirmation link has expired</h1>
            </div>
            {renderBody}
        </div>
    );
}

export default InactiveLink;