import React, { useState } from "react";
import axios from 'axios';

import "./RequestOTPForm.css";

const RequestOTPForm = () => {
    // States
    const [errorMessages, setErrorMessages] = useState({});
    const [isSubmitted, setIsSubmitted] = useState(false);
    const [checked, setChecked] = useState(false);

    const errors = {
        credentials: "Invalid username or password"
    };

    const renderErrorMessage = (name) =>
        name === errorMessages.name && (
            <div className="error">{errorMessages.message}</div>
        );

    const handleSubmit = (event) => {
        event.preventDefault();
        var { username, passphrase, authorizationLevel } = document.forms[0];


        axios.post('https://localhost:7010/OTPRequest/requestotp?=', {username, passphrase, authorizationLevel})
        .then(res => {
            
         })
    };

    const handleCheck = () => {
        setChecked(!checked);
    }

    const loginForm = (
        <div className="form-container">
            <div className="form-components">
                <form onSubmit={handleSubmit}>
                    <div className="input-container">
                        <input type="username" name="uname" required placeholder="Username" />
                        {renderErrorMessage("uname")}
                    </div>
                    <div className="input-container">
                        <input type="password" name="pass" required placeholder="Passphrase" />
                        {renderErrorMessage("pass")}
                    </div>
                    <div className="input-container">
                        <input type="authorizationLevel" name="authLevel" required placeholder="AuthorizationLevel" />
                        {renderErrorMessage("authLevel")}
                    </div>


                    <div className="create-button-container">
                        <input type="submit" value="Create" />
                    </div>
                </form>
            </div>
        </div>
        
    );

    return (
        <div className="form">
            <div className="title-text">
                <h1 className="login-title">Log In</h1>
            </div>
            {loginForm}
        </div>
    );
}

export default RequestOTPForm;